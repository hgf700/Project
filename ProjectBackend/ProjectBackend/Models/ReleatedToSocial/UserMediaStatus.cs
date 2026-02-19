using ProjectBackend.Models.ReleatedToMovie;

namespace ProjectBackend.Models.ReleatedToSocial;

//nie wiem cczy to dobre

public enum RatingValue
{
    Bad = -1,
    Neutral = 0,
    Good = 2
}

public class UserMediaStatus
{
    public int Id { get; set; }

    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

    public int MovieId { get; set; }
    public Movie Movie { get; set; }
    public RatingValue Rating { get; set; }  
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
