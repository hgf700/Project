using ProjectBackend.Models.ReleatedToMovie;

namespace ProjectBackend.Models.RelatedToRecommendation;

public class RecomendTagMovie
{
    public int MovieId { get; set; }
    public Movie Movie { get; set; }
    public int RecomendTagId{ get; set; }
    public RecomendTag RecomendTag { get; set; }
}
