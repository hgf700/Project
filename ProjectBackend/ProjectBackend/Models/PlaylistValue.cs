using Microsoft.Build.Tasks;

namespace ProjectBackend.Models;

public class PlaylistValue
{
    public int PlaylistId { get; set; }
    public Playlist Playlist { get; set; }

    public int MovieId { get; set; }
    public Movie Movie { get; set; }
}
