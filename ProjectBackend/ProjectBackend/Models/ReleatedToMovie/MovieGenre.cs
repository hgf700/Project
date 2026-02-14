using Humanizer.Localisation;

namespace ProjectBackend.Models.ReleatedToMovie;

public class MovieGenre
{
    public int MovieId { get; set; }
    public Movie Movie { get; set; } = null!;

    public int GenreId { get; set; }
    public Genre Genre { get; set; } = null!;
    public string MovieTitle { get; set; }
    //wyjebac potem genre name i moze movie title
    public string GenreName{ get; set; }

}
