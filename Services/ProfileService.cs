using System.Text.Json;
using Microsoft.JSInterop;

namespace BodyQuest.Services;

/// <summary>Everything the Profile page needs: the explorer card, real stats, worlds and badges.</summary>
public sealed class ProfileService
{
    private const string NameKey = "bodyquest.profile.name";
    private const string AvatarKey = "bodyquest.profile.avatar";
    private const string SeenKey = "bodyquest.badges.seen";
    private const string DefaultName = "Little Scientist";
    private const int XpPerLevel = 60;

    private readonly IJSRuntime _js;
    private readonly ModuleService _modules;
    private readonly WorldProgressService _progress;

    public ProfileService(IJSRuntime js, ModuleService modules, WorldProgressService progress)
    {
        _js = js;
        _modules = modules;
        _progress = progress;
    }

    // ---------- models ----------

    public sealed record AvatarOption(string Id, string Path, string Label);

    public sealed class WorldSummary
    {
        public string Slug { get; init; } = "";
        public string Title { get; init; } = "";
        public string Image { get; init; } = "";
        public string Accent { get; init; } = "#8b6fe0";
        public int LessonsDone { get; init; }
        public int LessonsTotal { get; init; }
        public int Stars { get; init; }
        public bool Unlocked { get; init; }
        public bool Completed { get; init; }
        public bool Current { get; init; }
        public int Percent => LessonsTotal == 0 ? 0 : (int)Math.Round(LessonsDone * 100.0 / LessonsTotal);
    }

    public sealed class BadgeInfo
    {
        public string Id { get; init; } = "";
        public string Name { get; init; } = "";
        public string Hint { get; init; } = "";
        public string Image { get; init; } = "";
        public bool Earned { get; init; }
    }

    public sealed class ExplorerProfile
    {
        public string Name { get; init; } = DefaultName;
        public AvatarOption Avatar { get; init; } = Avatars[0];
        public int Stars { get; init; }
        public int LessonsDone { get; init; }
        public int LessonsTotal { get; init; }
        public int WorldsDone { get; init; }
        public int Xp { get; init; }
        public int Level { get; init; }
        public int LevelPercent { get; init; }
        public string Rank { get; init; } = "";
        public List<WorldSummary> Worlds { get; init; } = new();
        public List<BadgeInfo> Badges { get; init; } = new();
        public int BadgesEarned => Badges.Count(b => b.Earned);
        public WorldSummary? CurrentWorld => Worlds.FirstOrDefault(w => w.Current);
    }

    public static readonly IReadOnlyList<AvatarOption> Avatars = new List<AvatarOption>
    {
        new("milo", "/images/body-explorer.png", "Milo"),
        new("brain", "/images/brain.png", "Brainy"),
        new("heart", "/images/heart.png", "Heartie"),
        new("lungs", "/images/lungs.png", "Breezy"),
        new("tummy", "/images/digestive.png", "Tummy"),
        new("bones", "/images/skeleton.png", "Bonesy"),
        new("cell", "/images/blood-cell.png", "Red Cell"),
        new("neuron", "/images/neuron.png", "Zappy"),
    };

    // ---------- load ----------

    public async Task<ExplorerProfile> LoadAsync()
    {
        var modules = await _modules.GetAllAsync();
        var completed = await _progress.GetCompletedAsync();
        var starMap = await _progress.GetStarsAsync();

        var worlds = new List<WorldSummary>();
        var previousDone = true;
        var currentAssigned = false;

        foreach (var m in modules)
        {
            var keys = m.Lessons.Select(l => WorldProgressService.MakeKey(m.Slug, l.Id)).ToList();
            var done = keys.Count(completed.Contains);
            var isComplete = keys.Count > 0 && done == keys.Count;
            var unlocked = previousDone;
            var art = WorldArt.For(m.Slug);

            worlds.Add(new WorldSummary
            {
                Slug = m.Slug,
                Title = m.Title,
                Image = art.Image,
                Accent = art.Accent,
                LessonsDone = done,
                LessonsTotal = keys.Count,
                Stars = keys.Where(completed.Contains).Sum(k => starMap.TryGetValue(k, out var s) ? s : 1),
                Unlocked = unlocked,
                Completed = isComplete,
                Current = unlocked && !isComplete && !currentAssigned
            });

            if (unlocked && !isComplete) currentAssigned = true;
            previousDone = isComplete;
        }

        var lessonsDone = worlds.Sum(w => w.LessonsDone);
        var lessonsTotal = worlds.Sum(w => w.LessonsTotal);
        var stars = worlds.Sum(w => w.Stars);
        var worldsDone = worlds.Count(w => w.Completed);

        var xp = stars * 10 + lessonsDone * 5;
        var level = xp / XpPerLevel + 1;
        var levelPercent = (int)Math.Round(xp % XpPerLevel * 100.0 / XpPerLevel);

        var hasPerfect = completed.Any(k => starMap.TryGetValue(k, out var s) && s >= 3);

        var badges = new List<BadgeInfo>
        {
            new() { Id = "first-steps", Name = "First Steps", Hint = "Learn your very first lesson",
                    Image = "/images/body-explorer.png", Earned = lessonsDone >= 1 },
            new() { Id = "perfect", Name = "Perfect Score", Hint = "Get 3 stars on a quick check",
                    Image = "/images/neuron.png", Earned = hasPerfect }
        };

        var worldBadges = new Dictionary<string, (string Name, string Image)>
        {
            ["cerveau"] = ("Brain Genius", "/images/brain.png"),
            ["coeur"] = ("Big Heart", "/images/heart.png"),
            ["poumons"] = ("Deep Breath", "/images/lungs.png"),
            ["digestion"] = ("Tummy Expert", "/images/digestive.png"),
            ["os-muscles"] = ("Strong Bones", "/images/skeleton.png"),
            ["cinq-sens"] = ("Super Senses", "/images/learning-worlds/senses-world.webp"),
        };

        foreach (var w in worlds)
        {
            if (!worldBadges.TryGetValue(w.Slug, out var b)) continue;
            badges.Add(new BadgeInfo { Id = "world-" + w.Slug, Name = b.Name, Hint = $"Finish {w.Title}", Image = b.Image, Earned = w.Completed });
        }

        badges.Add(new BadgeInfo
        {
            Id = "body-hero",
            Name = "Body Hero",
            Hint = "Finish every world",
            Image = "/images/portal.png",
            Earned = worlds.Count > 0 && worldsDone == worlds.Count
        });

        return new ExplorerProfile
        {
            Name = await GetNameAsync(),
            Avatar = await GetAvatarAsync(),
            Stars = stars,
            LessonsDone = lessonsDone,
            LessonsTotal = lessonsTotal,
            WorldsDone = worldsDone,
            Xp = xp,
            Level = level,
            LevelPercent = levelPercent,
            Rank = RankFor(level),
            Worlds = worlds,
            Badges = badges
        };
    }

    public static string RankFor(int level) => level switch
    {
        <= 1 => "Curious Cadet",
        2 => "Young Explorer",
        3 => "Body Scout",
        4 => "Super Scientist",
        5 => "Anatomy Ace",
        _ => "Body Hero"
    };

    // ---------- name and avatar ----------

    public async Task<string> GetNameAsync()
    {
        var raw = await ReadAsync(NameKey);
        return string.IsNullOrWhiteSpace(raw) ? DefaultName : Clean(raw);
    }

    public async Task<AvatarOption> GetAvatarAsync()
    {
        var id = await ReadAsync(AvatarKey);
        return Avatars.FirstOrDefault(a => a.Id == id) ?? Avatars[0];
    }

    /// <summary>Saves the explorer's name (1 to 16 characters) and avatar. Returns false if the name is empty.</summary>
    public async Task<bool> SaveAsync(string? name, string? avatarId)
    {
        var clean = Clean(name);
        if (clean.Length == 0) return false;
        var avatar = Avatars.FirstOrDefault(a => a.Id == avatarId) ?? Avatars[0];
        await WriteAsync(NameKey, clean);
        await WriteAsync(AvatarKey, avatar.Id);
        return true;
    }

    public static string Clean(string? name)
    {
        var parts = (name ?? "").Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var joined = string.Join(' ', parts);
        return joined.Length > 16 ? joined[..16].TrimEnd() : joined;
    }

    // ---------- badges already celebrated ----------

    public async Task<List<BadgeInfo>> GetNewBadgesAsync(ExplorerProfile profile)
    {
        var seen = await ReadSeenAsync();
        var earned = profile.Badges.Where(b => b.Earned).ToList();
        if (earned.Count == 0 && seen.Count > 0)
        {
            await WriteAsync(SeenKey, "[]"); // progress was erased: start fresh
            return new();
        }
        return earned.Where(b => !seen.Contains(b.Id)).ToList();
    }

    public async Task MarkBadgesSeenAsync(IEnumerable<BadgeInfo> badges)
    {
        var seen = await ReadSeenAsync();
        foreach (var b in badges) seen.Add(b.Id);
        await WriteAsync(SeenKey, JsonSerializer.Serialize(seen));
    }

    private async Task<HashSet<string>> ReadSeenAsync()
    {
        var raw = await ReadAsync(SeenKey);
        try
        {
            return string.IsNullOrWhiteSpace(raw)
                ? new()
                : new(JsonSerializer.Deserialize<List<string>>(raw) ?? new());
        }
        catch (JsonException) { return new(); }
    }

    // ---------- browser storage (the app keeps working if storage is blocked) ----------

    private async Task<string?> ReadAsync(string key)
    {
        try { return await _js.InvokeAsync<string?>("bodyQuestWorlds.loadValue", key); }
        catch (JSException) { return null; }
        catch (InvalidOperationException) { return null; }
        catch (JSDisconnectedException) { return null; }
    }

    private async Task WriteAsync(string key, string value)
    {
        try { await _js.InvokeVoidAsync("bodyQuestWorlds.saveValue", key, value); }
        catch (JSException) { }
        catch (InvalidOperationException) { }
        catch (JSDisconnectedException) { }
    }
}