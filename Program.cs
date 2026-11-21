using BodyQuest.Auth;
using BodyQuest.Components;
using BodyQuest.Data;
using BodyQuest.Models;
using BodyQuest.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "The DefaultConnection connection string is missing."
    );

builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    )
);

// Identity (UserManager / SignInManager) needs the DbContext itself, not only the factory.
// Blazor components keep using IDbContextFactory; Identity gets one context per HTTP request.
builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext());

// ---------- Accounts for children ----------
// Nickname + avatar + 4-digit secret code. No e-mail, no real name.
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthorization();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

builder.Services
    .AddIdentityCore<Player>(options =>
    {
        // The "password" is a 4-digit code, so the usual complexity rules are switched off.
        options.Password.RequiredLength = 4;
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredUniqueChars = 1;

        // Nicknames may contain accents, Arabic letters and spaces (they are checked in AuthEndpoints).
        options.User.AllowedUserNameCharacters = string.Empty;
        options.User.RequireUniqueEmail = false;

        // A 4-digit code can be guessed, so a few wrong tries lock the account for a short time.
        options.Lockout.AllowedForNewUsers = true;
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddSignInManager();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "bodyquest.player";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.SlidingExpiration = true;
    // No page is protected, so never redirect to a login page: just answer with a status code.
    options.Events.OnRedirectToLogin = ctx => { ctx.Response.StatusCode = 401; return Task.CompletedTask; };
    options.Events.OnRedirectToAccessDenied = ctx => { ctx.Response.StatusCode = 403; return Task.CompletedTask; };
});

// Application services
builder.Services.AddScoped<ModuleService>();
builder.Services.AddScoped<WorldProgressService>();
builder.Services.AddScoped<ProfileService>();
builder.Services.AddScoped<PlayerSession>();

// New multilingual service (French / English / Arabic)
builder.Services.AddScoped<LanguageService>();

var app = builder.Build();

// Initialize database and seed initial content.
using (var scope = app.Services.CreateScope())
{
    var factory = scope.ServiceProvider
        .GetRequiredService<IDbContextFactory<AppDbContext>>();

    await using var db = await factory.CreateDbContextAsync();

    // With migrations in the project, apply them. Without any, create the schema directly.
    if (db.Database.GetMigrations().Any())
        await db.Database.MigrateAsync();
    else
        await db.Database.EnsureCreatedAsync();

    await SeedData.InitializeAsync(db);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapAuthEndpoints();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
