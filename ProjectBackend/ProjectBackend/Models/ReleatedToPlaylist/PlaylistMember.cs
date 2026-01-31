using ProjectBackend.Models.ReleatedToSocial;

namespace ProjectBackend.Models.ReleatedToPlaylist;

public enum PlaylistRole
{
    Owner = 0,
    Editor = 1,
    Viewer = 2
}

public class PlaylistMember
{
    public int PlaylistId { get; set; }
    public Playlist Playlist { get; set; }

    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

    public PlaylistRole Role { get; set; }
}
