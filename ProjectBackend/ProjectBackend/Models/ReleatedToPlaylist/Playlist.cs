using ProjectBackend.Models.ReleatedToSocial;

namespace ProjectBackend.Models.ReleatedToPlaylist;


public class Playlist
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    public bool IsPublic { get; set; }
    public ICollection<PlaylistLike> Likes { get; set; } = new List<PlaylistLike>();
    public ICollection<PlaylistComment> Comments { get; set; } = new List<PlaylistComment>();
    public ICollection<PlaylistMember> Members { get; set; } = new List<PlaylistMember>();

}
