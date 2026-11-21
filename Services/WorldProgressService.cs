using System.Text.Json;
using BodyQuest.Data;
using BodyQuest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;

namespace BodyQuest.Services;

/// <summary>
/// Remembers which lessons the child has finished (and how many stars they earned).
/// A signed-in child's progress is saved in the database, so it follows them to any device.
/// A guest's progress stays in the browser (localStorage), as before.
/// Also wraps the sound, music and confetti helpers in bodyquest-worlds.js.
/// </summary>
public sealed class WorldProgressService
{
    private readonly IJSRuntime _js;
    private readonly PlayerSession _session;
    private readonly IDbContextFactory<AppDbContext> _factory;

    private const string StorageKey = "bodyquest.learning.completed.v2";
    private const string StarsKey = "bodyquest.learning.stars.v1";
    private const string MiloKey = "bodyquest.milo.v1";

    private HashSet<string>? _completed;
    private Dictionary<string, int>? _stars;
    private string? _userId;
    private bool _userKnown;

    public WorldProgressService(IJSRuntime js, PlayerSession session, IDbContextFactory<AppDbContext> factory)
    {
        _js = js;
        _session = session;
        _factory = factory;
    }

    // ---------- who is playing ----------

    private async Task<string?> UserIdAsync()
    {
        if (_userKnown) return _userId;
        try { _userId = await _session.GetUserIdAsync(); }
        catch (InvalidOperationException) { _userId = null; }
        _userKnown = true;
        return _userId;
    }

    /// <summary>Loads the progress once: from the database for a signed-in child, from the browser for a guest.</summary>
    private async Task EnsureLoadedAsync()
    {
        if (_completed is not null && _stars is not null) return;

        var uid = await UserIdAsync();
        if (uid is not null)
        {
            await using var db = await _factory.CreateDbContextAsync();
            var rows = await db.LessonProgresses.AsNoTracking().Where(p => p.UserId == uid).ToListAsync();

            _completed = new HashSet<string>(
                rows.Select(r => MakeKey(r.WorldSlug, r.LessonId)), StringComparer.OrdinalIgnoreCase);
            _stars = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            foreach (var r in rows) _stars[MakeKey(r.WorldSlug, r.LessonId)] = Math.Clamp(r.Stars, 1, 3);
            return;
        }

        var (done, stars) = await GetGuestSnapshotAsync();
        _completed = done.ToHashSet(StringComparer.OrdinalIgnoreCase);
        _stars = new Dictionary<string, int>(stars, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>What this browser has saved as a guest (used to keep the stars when a child creates an account).</summary>
    public async Task<(string[] Done, Dictionary<string, int> Stars)> GetGuestSnapshotAsync()
    {
        var keys = await SafeAsync(() => _js.InvokeAsync<string[]>("bodyQuestWorlds.loadProgress", StorageKey).AsTask())
                   ?? Array.Empty<string>();

        var json = await SafeAsync(() => _js.InvokeAsync<string?>("bodyQuestWorlds.loadValue", StarsKey).AsTask());
        var map = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        try
        {
            if (!string.IsNullOrWhiteSpace(json))
                map = new(JsonSerializer.Deserialize<Dictionary<string, int>>(json!) ?? new(), StringComparer.OrdinalIgnoreCase);
        }
        catch (JsonException) { }

        return (keys, map);
    }

    // ---------- completed lessons ----------

    public async Task<HashSet<string>> GetCompletedAsync()
    {
        await EnsureLoadedAsync();
        return new HashSet<string>(_completed!, StringComparer.OrdinalIgnoreCase);
    }

    public async Task MarkCompletedAsync(string worldSlug, int lessonId, int stars = 1)
    {
        await EnsureLoadedAsync();

        stars = Math.Clamp(stars, 1, 3);
        var key = MakeKey(worldSlug, lessonId);
        var uid = await UserIdAsync();

        _completed!.Add(key);
        if (!_stars!.TryGetValue(key, out var old) || stars > old) _stars[key] = stars;

        if (uid is not null)
        {
            try
            {
                await using var db = await _factory.CreateDbContextAsync();
                var row = await db.LessonProgresses.FirstOrDefaultAsync(
                    p => p.UserId == uid && p.WorldSlug == worldSlug && p.LessonId == lessonId);

                if (row is null)
                    db.LessonProgresses.Add(new LessonProgress
                    {
                        UserId = uid, WorldSlug = worldSlug, LessonId = lessonId,
                        Stars = stars, CompletedAt = DateTime.UtcNow
                    });
                else if (stars > row.Stars)
                    row.Stars = stars;

                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // The same lesson was saved twice at once (double tap): the first save is enough.
            }
            return;
        }

        await SafeAsync(() => _js.InvokeVoidAsync("bodyQuestWorlds.saveProgress", StorageKey, _completed.ToArray()).AsTask());
        await SafeAsync(() => _js.InvokeVoidAsync("bodyQuestWorlds.saveValue", StarsKey, JsonSerializer.Serialize(_stars)).AsTask());
    }

    // ---------- stars (1 to 3 per lesson) ----------

    public async Task<Dictionary<string, int>> GetStarsAsync()
    {
        await EnsureLoadedAsync();
        return new Dictionary<string, int>(_stars!, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>Total stars: lessons finished before stars existed count for one.</summary>
    public async Task<int> GetTotalStarsAsync()
    {
        var done = await GetCompletedAsync();
        var stars = await GetStarsAsync();
        return done.Sum(k => stars.TryGetValue(k, out var s) ? s : 1);
    }

    // ---------- Milo's position on the map ----------

    // Each signed-in child has their own spot on the map.
    private async Task<string> MiloStorageKeyAsync()
    {
        var uid = await UserIdAsync();
        return uid is null ? MiloKey : $"{MiloKey}.{uid}";
    }

    public async Task<int> GetMiloStopAsync()
    {
        var key = await MiloStorageKeyAsync();
        var raw = await SafeAsync(() => _js.InvokeAsync<string?>("bodyQuestWorlds.loadValue", key).AsTask());
        return int.TryParse(raw, out var i) ? Math.Max(0, i) : 0;
    }

    public async Task SetMiloStopAsync(int index)
    {
        var key = await MiloStorageKeyAsync();
        await SafeAsync(() => _js.InvokeVoidAsync("bodyQuestWorlds.saveValue", key, index.ToString()).AsTask());
    }

    // ---------- reset ----------

    public async Task ResetAsync()
    {
        var uid = await UserIdAsync();
        _completed = new(StringComparer.OrdinalIgnoreCase);
        _stars = new(StringComparer.OrdinalIgnoreCase);

        if (uid is not null)
        {
            await using var db = await _factory.CreateDbContextAsync();
            await db.LessonProgresses.Where(p => p.UserId == uid).ExecuteDeleteAsync();
        }
        else
        {
            await SafeAsync(() => _js.InvokeVoidAsync("bodyQuestWorlds.saveProgress", StorageKey, Array.Empty<string>()).AsTask());
            await SafeAsync(() => _js.InvokeVoidAsync("bodyQuestWorlds.saveValue", StarsKey, "{}").AsTask());
        }

        var key = await MiloStorageKeyAsync();
        await SafeAsync(() => _js.InvokeVoidAsync("bodyQuestWorlds.saveValue", key, "0").AsTask());
    }

    // ---------- sounds, music, confetti ----------

    public Task PlayAsync(string name) =>
        SafeAsync(() => _js.InvokeVoidAsync("bodyQuestWorlds.play", name).AsTask());

    public Task PlayCompletionSoundAsync() => PlayAsync("lesson");

    public Task ConfettiAsync(bool big = false) =>
        SafeAsync(() => _js.InvokeVoidAsync("bodyQuestWorlds.confetti", big).AsTask());

    public async Task<bool> IsMutedAsync() =>
        await SafeAsync(() => _js.InvokeAsync<bool>("bodyQuestWorlds.isMuted").AsTask());

    public async Task<bool> SetMutedAsync(bool muted) =>
        await SafeAsync(() => _js.InvokeAsync<bool>("bodyQuestWorlds.setMuted", muted).AsTask());

    public async Task<bool> WasMusicOnAsync() =>
        await SafeAsync(() => _js.InvokeAsync<bool>("bodyQuestWorlds.wasMusicOn").AsTask());

    public async Task<bool> SetMusicAsync(bool on) =>
        await SafeAsync(() => _js.InvokeAsync<bool>("bodyQuestWorlds.setMusic", on).AsTask());

    public static string MakeKey(string worldSlug, int lessonId) => $"{worldSlug}::{lessonId}";

    // The app keeps working if the browser blocks storage or audio.
    private static async Task<T?> SafeAsync<T>(Func<Task<T>> call)
    {
        try { return await call(); }
        catch (JSException) { return default; }
        catch (InvalidOperationException) { return default; }
        catch (JSDisconnectedException) { return default; }
    }

    private static async Task SafeAsync(Func<Task> call)
    {
        try { await call(); }
        catch (JSException) { }
        catch (InvalidOperationException) { }
        catch (JSDisconnectedException) { }
    }
}
