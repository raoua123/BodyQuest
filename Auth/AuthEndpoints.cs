using System.Security.Claims;
using BodyQuest.Data;
using BodyQuest.Models;
using BodyQuest.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BodyQuest.Auth;

public static class AuthEndpoints
{
    public sealed record RegisterRequest(
        string? Nickname, string? Pin, string? AvatarId,
        string[]? Done, Dictionary<string, int>? Stars);

    public sealed record LoginRequest(string? Nickname, string? Pin);

    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth");
        group.MapPost("/register", RegisterAsync);
        group.MapPost("/login", LoginAsync);
        group.MapPost("/logout", LogoutAsync);

        // Plain page navigation: works even if the JavaScript helper did not load.
        group.MapGet("/logout", async (SignInManager<Player> signIn) =>
        {
            await signIn.SignOutAsync();
            return Results.Redirect("/");
        });

        return app;
    }

    private static async Task<IResult> RegisterAsync(
        RegisterRequest req,
        UserManager<Player> users,
        SignInManager<Player> signIn,
        IDbContextFactory<AppDbContext> factory)
    {
        var name = CleanName(req.Nickname);
        if (name is null) return Fail("name");
        if (!IsPin(req.Pin)) return Fail("pin");

        var avatarId = ProfileService.Avatars.FirstOrDefault(a => a.Id == req.AvatarId)?.Id
                       ?? ProfileService.Avatars[0].Id;

        var user = new Player { UserName = name, AvatarId = avatarId, CreatedAt = DateTime.UtcNow };

        try
        {
            var created = await users.CreateAsync(user, req.Pin!);
            if (!created.Succeeded)
            {
                var taken = created.Errors.Any(e => e.Code == "DuplicateUserName");
                return Fail(taken ? "taken" : "name");
            }
        }
        catch (DbUpdateException)
        {
            return Fail("taken");
        }

        await ImportGuestProgressAsync(factory, user.Id, req.Done, req.Stars);
        await SignInAsync(signIn, user);
        return Results.Ok(new { ok = true });
    }

    private static async Task<IResult> LoginAsync(
        LoginRequest req,
        UserManager<Player> users,
        SignInManager<Player> signIn)
    {
        var name = CleanName(req.Nickname);
        if (name is null || !IsPin(req.Pin)) return Fail("wrong");

        var user = await users.FindByNameAsync(name);
        if (user is null) return Fail("wrong");

        var result = await signIn.CheckPasswordSignInAsync(user, req.Pin!, lockoutOnFailure: true);
        if (result.IsLockedOut) return Fail("locked");
        if (!result.Succeeded) return Fail("wrong");

        await SignInAsync(signIn, user);
        return Results.Ok(new { ok = true });
    }

    private static async Task<IResult> LogoutAsync(SignInManager<Player> signIn)
    {
        await signIn.SignOutAsync();
        return Results.Ok(new { ok = true });
    }

    private static Task SignInAsync(SignInManager<Player> signIn, Player user) =>
        signIn.SignInWithClaimsAsync(
            user,
            isPersistent: true,
            new[] { new Claim(PlayerSession.AvatarClaim, user.AvatarId) });

    private static IResult Fail(string code) => Results.BadRequest(new { ok = false, error = code });

    public static string? CleanName(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return null;

        var parts = raw.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var name = string.Join(' ', parts);
        if (name.Length < 2 || name.Length > 16) return null;

        foreach (var c in name)
            if (!(char.IsLetterOrDigit(c) || c == ' ' || c == '-' || c == '_')) return null;

        return name;
    }

    private static bool IsPin(string? pin) =>
        pin is { Length: 4 } && pin.All(c => c >= '0' && c <= '9');

    // The login page now sends no guest progress, so a new account always starts at zero.
    private static async Task ImportGuestProgressAsync(
        IDbContextFactory<AppDbContext> factory, string userId,
        string[]? done, Dictionary<string, int>? stars)
    {
        if (done is null || done.Length == 0) return;

        await using var db = await factory.CreateDbContextAsync();

        var lessons = await db.Lessons
            .AsNoTracking()
            .Select(l => new { l.Id, Slug = l.BodyModule!.Slug })
            .ToListAsync();

        var valid = lessons.ToDictionary(
            l => WorldProgressService.MakeKey(l.Slug, l.Id),
            l => (l.Slug, l.Id),
            StringComparer.OrdinalIgnoreCase);

        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var key in done.Take(300))
        {
            if (!valid.TryGetValue(key, out var lesson) || !seen.Add(key)) continue;

            var earned = stars is not null && stars.TryGetValue(key, out var s) ? Math.Clamp(s, 1, 3) : 1;
            db.LessonProgresses.Add(new LessonProgress
            {
                UserId = userId,
                WorldSlug = lesson.Slug,
                LessonId = lesson.Id,
                Stars = earned,
                CompletedAt = DateTime.UtcNow
            });
        }

        await db.SaveChangesAsync();
    }
}