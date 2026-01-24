namespace ProjectBackend.Models.DTO;

public class MovieRatingDto
{
    public int MovieId { get; set; }
    public string RatingLabel { get; set; }
    public int RatingNumeric { get; set; }
}

public class UserMovieRating
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int MovieId { get; set; }
    public int Rating { get; set; } // Bad / Neutral / Good
    public int Points => (int)Rating;       // 1 / 3 / 10
}
