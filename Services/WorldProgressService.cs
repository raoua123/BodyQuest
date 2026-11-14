using System.Text.Json;
using Microsoft.JSInterop;

namespace BodyQuest.Services;

/// <summary>
/// Remembers which lessons the child has finished (and how many stars they earned)
/// in the browser. Also wraps the sound, music and confetti helpers in bodyquest-worlds.js.
/// </summary>
public sealed class WorldProgressService
{
    private readonly IJSRuntime _js;

    private const string StorageKey = "bodyquest.learning.completed.v2";
    private const string StarsKey = "bodyquest.learning.stars.v1";
    private const string MiloKey = "bodyquest.milo.v1";

    private HashSet<string>? _completed;
    private Dictionary<string, int>? _stars;

    public WorldProgressService(IJSRuntime js) => _js = js;

    // ---------- completed lessons ----------

    public async Task<HashSet<string>> GetCompletedAsync()
    {
        if (_completed is null)
        {
            var keys = await SafeAsync(() => _js.InvokeAsync<string[]>("bodyQuestWorlds.loadProgress", StorageKey).AsTask())
                       ?? Array.Empty<string>();
            _completed = keys.ToHashSet(StringComparer.OrdinalIgnoreCase);
        }
        return new HashSet<string>(_completed, StringComparer.OrdinalIgnoreCase);
    }

    public async Task MarkCompletedAsync(string worldSlug, int lessonId, int stars = 1)
    {
        var completed = await GetCompletedAsync();
        completed.Add(MakeKey(worldSlug, lessonId));
        _completed = completed;
        await SafeAsync(() => _js.InvokeVoidAsync("bodyQuestWorlds.saveProgress", StorageKey, completed.ToArray()).AsTask());

        var map = await GetStarsAsync();
        var key = MakeKey(worldSlug, lessonId);
        if (!map.TryGetValue(key, out var old) || stars > old)
        {
            map[key] = Math.Clamp(stars, 1, 3);
            _stars = map;
            await SafeAsync(() => _js.InvokeVoidAsync("bodyQuestWorlds.saveValue", StarsKey, JsonSerializer.Serialize(map)).AsTask());
        }
    }

    // ---------- stars (1 to 3 per lesson) ----------

    public async Task<Dictionary<string, int>> GetStarsAsync()
    {
        if (_stars is null)
        {
            var json = await SafeAsync(() => _js.InvokeAsync<string?>("bodyQuestWorlds.loadValue", StarsKey).AsTask());
            try
            {
                _stars = string.IsNullOrWhiteSpace(json)
                    ? new(StringComparer.OrdinalIgnoreCase)
                    : new(JsonSerializer.Deserialize<Dictionary<string, int>>(json!) ?? new(), StringComparer.OrdinalIgnoreCase);
            }
            catch (JsonException) { _stars = new(StringComparer.OrdinalIgnoreCase); }
        }
        return new Dictionary<string, int>(_stars, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>Total stars: lessons finished before stars existed count for one.</summary>
    public async Task<int> GetTotalStarsAsync()
    {
        var done = await GetCompletedAsync();
        var stars = await GetStarsAsync();
        return done.Sum(k => stars.TryGetValue(k, out var s) ? s : 1);
    }

    // ---------- Milo's position on the map ----------

    public async Task<int> GetMiloStopAsync()
    {
        var raw = await SafeAsync(() => _js.InvokeAsync<string?>("bodyQuestWorlds.loadValue", MiloKey).AsTask());
        return int.TryParse(raw, out var i) ? Math.Max(0, i) : 0;
    }

    public Task SetMiloStopAsync(int index) =>
        SafeAsync(() => _js.InvokeVoidAsync("bodyQuestWorlds.saveValue", MiloKey, index.ToString()).AsTask());

    // ---------- reset ----------

    public async Task ResetAsync()
    {
        _completed = new(StringComparer.OrdinalIgnoreCase);
        _stars = new(StringComparer.OrdinalIgnoreCase);
        await SafeAsync(() => _js.InvokeVoidAsync("bodyQuestWorlds.saveProgress", StorageKey, Array.Empty<string>()).AsTask());
        await SafeAsync(() => _js.InvokeVoidAsync("bodyQuestWorlds.saveValue", StarsKey, "{}").AsTask());
        await SafeAsync(() => _js.InvokeVoidAsync("bodyQuestWorlds.saveValue", MiloKey, "0").AsTask());
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
