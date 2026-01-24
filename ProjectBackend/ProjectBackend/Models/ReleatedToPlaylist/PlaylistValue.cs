using Microsoft.Build.Tasks;
using ProjectBackend.Models.ReleatedToMovie;

namespace ProjectBackend.Models.ReleatedToPlaylist;

public class PlaylistValue
{
    public int PlaylistId { get; set; }
    public Playlist Playlist { get; set; }

    public int MovieId { get; set; }
    public Movie Movie { get; set; }
}
