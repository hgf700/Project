namespace ProjectBackend.Models;

//https://developer.themoviedb.org/reference/collection-details
//http://developer.themoviedb.org/reference/collection-images
public class Movie
{
    public int Id { get; set; }
    public string name{ get; set; }
    public string overview { get; set; }
    public bool adult{ get; set; }
    public int[] genre_ids {  get; set; }
    public string release_date { get; set; }
    public float vote_average { get; set; }
    public string poster_path { get; set; }

    public ICollection<MovieGenre> MovieGenres { get; set; }
}
