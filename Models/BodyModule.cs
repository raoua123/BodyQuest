namespace BodyQuest.Models;

/// <summary>One of the six learning modules (brain, heart, lungs...).</summary>
public class BodyModule
{
    public int Id { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Emoji { get; set; } = string.Empty;
    public string ColorHex { get; set; } = "#4f8cff";
    public int SortOrder { get; set; }

    public List<Lesson> Lessons { get; set; } = new();
    public List<Organ> Organs { get; set; } = new();
}

/// <summary>A short illustrated lesson that belongs to a module.</summary>
public class Lesson
{
    public int Id { get; set; }
    public int BodyModuleId { get; set; }
    public BodyModule? BodyModule { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}

/// <summary>An organ or body part. SvgId links it to a shape on the interactive body map (Task 2).</summary>
public class Organ
{
    public int Id { get; set; }
    public int BodyModuleId { get; set; }
    public BodyModule? BodyModule { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string SvgId { get; set; } = string.Empty;
}

/// <summary>A reward the child can earn (used from Task 6).</summary>
public class Badge
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Emoji { get; set; } = "⭐";
}
