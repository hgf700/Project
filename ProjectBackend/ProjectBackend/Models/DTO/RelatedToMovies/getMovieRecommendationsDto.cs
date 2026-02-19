namespace ProjectBackend.Models.DTO.RelatedToMovies;

public class getMovieRecommendationsDto
{
    public int TmdbId {  get; set; }
    public string Title { get; set; }
    public string Overview { get; set; }
    public string PosterPath { get; set; }
    public double MovieRecommendations { get; set; }

}
