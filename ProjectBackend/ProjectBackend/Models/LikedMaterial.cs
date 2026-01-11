namespace ProjectBackend.Models;

public class LikedMaterial
{
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

    public int MovieId { get; set; }
    public Movie Movie { get; set; }
}
