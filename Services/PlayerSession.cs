using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace BodyQuest.Services;

/// <summary>Who is playing right now (a signed-in child), as shown in the header.</summary>
public sealed record PlayerInfo(string Id, string Name, string AvatarId)
{
    public string AvatarPath =>
        (ProfileService.Avatars.FirstOrDefault(a => a.Id == AvatarId) ?? ProfileService.Avatars[0]).Path;
}

/// <summary>
/// Reads the signed-in child from the login cookie. Returns null for a guest.
/// Signing in and out happens in AuthEndpoints, because a cookie can only be set by a normal HTTP request.
/// </summary>
public sealed class PlayerSession
{
    public const string AvatarClaim = "bodyquest:avatar";

    private readonly AuthenticationStateProvider _auth;

    public PlayerSession(AuthenticationStateProvider auth) => _auth = auth;

    public async Task<PlayerInfo?> GetAsync()
    {
        var state = await _auth.GetAuthenticationStateAsync();
        var user = state.User;
        if (user.Identity?.IsAuthenticated != true) return null;

        var id = user.FindFirstValue(ClaimTypes.NameIdentifier);
        var name = user.FindFirstValue(ClaimTypes.Name);
        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name)) return null;

        return new PlayerInfo(id, name, user.FindFirstValue(AvatarClaim) ?? "milo");
    }

    public async Task<string?> GetUserIdAsync() => (await GetAsync())?.Id;
}
