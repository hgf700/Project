using ProjectBackend.Models.ReleatedToPlaylist;

namespace ProjectBackend.Models.DTO.RelatedToPlaylist;

public class getPlaylistFriendsDto
{
    public int PlaylistId { get; set; }
    public string UserId { get; set; }
    public PlaylistRole Role { get; set; }
    public string Email { get; set; }

}
