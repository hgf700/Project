namespace ProjectBackend.Models;

public class LikedMaterials
{
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

    public int MovieId { get; set; }
    public Movie Movie { get; set; }
}
