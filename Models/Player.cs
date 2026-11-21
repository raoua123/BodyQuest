using Microsoft.AspNetCore.Identity;

namespace BodyQuest.Models;

/// <summary>
/// A child's account. The nickname is the UserName and the 4-digit secret code is the password
/// (stored hashed by Identity). No e-mail, no real name: nothing personal is collected.
/// </summary>
public class Player : IdentityUser
{
    /// <summary>Id of one of the avatars in ProfileService.Avatars.</summary>
    public string AvatarId { get; set; } = "milo";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>A lesson a player has finished, with the stars earned (1 to 3).</summary>
public class LessonProgress
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;
    public Player? User { get; set; }

    public string WorldSlug { get; set; } = string.Empty;
    public int LessonId { get; set; }
    public int Stars { get; set; } = 1;
    public DateTime CompletedAt { get; set; } = DateTime.UtcNow;
}
