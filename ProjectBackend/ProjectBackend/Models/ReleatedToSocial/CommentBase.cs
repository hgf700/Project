using ProjectBackend.Models.ReleatedToPlaylist;
using ProjectBackend.Models.ReleatedToSocial;

public abstract class CommentBase
{
    public int Id { get; set; }
    public string Text { get; set; }
    public string UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ApplicationUser User { get; set; }
}

public class UserComment : CommentBase
{
    public string TargetUserId { get; set; }
    public ApplicationUser TargetUser { get; set; }
}

public class PlaylistComment : CommentBase
{
    public int PlaylistId { get; set; }
    public Playlist Playlist { get; set; }
}
