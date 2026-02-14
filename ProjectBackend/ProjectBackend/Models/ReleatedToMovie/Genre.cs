using ProjectBackend.Models.ReleatedToSocial;

namespace ProjectBackend.Models.ReleatedToMovie;

public class Genre
{
    public int Id { get; set; }
    public int TmdbId { get; set; }
    public string Name { get; set; }
    public ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
    public ICollection<PrefferedGenre> PrefferedGenres { get; set; }

}
