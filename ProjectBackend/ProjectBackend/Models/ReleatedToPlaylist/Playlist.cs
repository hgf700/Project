using ProjectBackend.Models.ReleatedToSocial;

namespace ProjectBackend.Models.ReleatedToPlaylist;


public class Playlist
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    public ICollection<PlaylistMember> Members { get; set; }

}
