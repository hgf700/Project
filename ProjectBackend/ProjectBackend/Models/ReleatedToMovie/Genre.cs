namespace ProjectBackend.Models;

public class Genre
{
    public int Id { get; set; }
    public int TmdbId { get; set; }
    public string Name { get; set; }
    public ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();

}
