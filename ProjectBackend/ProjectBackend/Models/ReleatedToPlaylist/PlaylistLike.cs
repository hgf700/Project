using ProjectBackend.Models.ReleatedToSocial;

namespace ProjectBackend.Models.ReleatedToPlaylist;

public class PlaylistLike
{
    public int PlaylistId { get; set; }
    public Playlist Playlist { get; set; }

    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
