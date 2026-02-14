using ProjectBackend.Models.ReleatedToMovie;
using ProjectBackend.Models.ReleatedToPlaylist;

namespace ProjectBackend.Models.ReleatedToSocial;

public class PrefferedGenre
{
    public int GenreId { get; set; }
    public Genre Genre{ get; set; }

    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
}
