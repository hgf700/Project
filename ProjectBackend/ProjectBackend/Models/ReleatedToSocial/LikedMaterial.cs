using ProjectBackend.Models.ReleatedToMovie;

namespace ProjectBackend.Models.ReleatedToSocial;

public class LikedMaterial
{
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

    public int MovieId { get; set; }
    public Movie Movie { get; set; }
}
