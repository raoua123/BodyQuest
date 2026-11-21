namespace BodyQuest.Services;

/// <summary>Artwork and colours for each world. Kept in code so the database schema does not change.</summary>
public static class WorldArt
{
    public sealed record Art(string Image, string Mascot, string Accent, string Soft, string SkyTop, string SkyBottom, string Tagline);

    private static readonly Dictionary<string, Art> Worlds = new(StringComparer.OrdinalIgnoreCase)
    {
        ["cerveau"] = new("/images/learning-worlds/brain-world.webp", "/images/brain.png",
            "#9a7be0", "#efe8ff", "#d9ccff", "#f6efff", "The control centre of your body"),
        ["coeur"] = new("/images/learning-worlds/heart-world.webp", "/images/heart.png",
            "#ef6b73", "#ffe8e9", "#ffd3d6", "#fff1f1", "The pump that never stops"),
        ["poumons"] = new("/images/learning-worlds/lung-world.webp", "/images/lungs.png",
            "#3fb5a3", "#e1f7f3", "#bff0e8", "#eefbf8", "Breathe in, breathe out"),
        ["digestion"] = new("/images/learning-worlds/digestion-world.webp", "/images/digestive.png",
            "#f0a23c", "#fff0d6", "#ffe0a8", "#fff8e8", "From the first bite to energy"),
        ["os-muscles"] = new("/images/learning-worlds/body-structure.webp", "/images/skeleton.png",
            "#7a8fe0", "#e8edff", "#cfd8ff", "#f3f5ff", "Strong bones, mighty muscles"),
        ["cinq-sens"] = new("/images/learning-worlds/senses-world.webp", "/images/learning-worlds/senses-world.webp",
            "#37b6c9", "#def6fa", "#b9ecf5", "#eefbfd", "See, hear, smell, taste and touch"),
        ["dents"] = new("/images/learning-worlds/teeth-world.svg", "/images/learning-worlds/teeth-world.svg",
            "#33b4c9", "#dcf6fa", "#bdeef5", "#eefbfd", "Tiny tools for biting and smiling"),
        ["peau"] = new("/images/learning-worlds/skin-world.svg", "/images/learning-worlds/skin-world.svg",
            "#ee8a6a", "#ffe9e0", "#ffd8c9", "#fff3ee", "Your coat that protects you"),
        ["defenses"] = new("/images/learning-worlds/defense-world.svg", "/images/learning-worlds/defense-world.svg",
            "#5a8de0", "#e4edff", "#cfe0ff", "#f2f6ff", "A tiny army that watches over you"),
    };

    private static readonly Art Fallback = new("/images/portal.png", "/images/portal.png",
        "#8174d9", "#efeaff", "#d9ccff", "#f6efff", "A new world to explore");

    public static Art For(string? slug) =>
        slug is not null && Worlds.TryGetValue(slug, out var art) ? art : Fallback;
}
