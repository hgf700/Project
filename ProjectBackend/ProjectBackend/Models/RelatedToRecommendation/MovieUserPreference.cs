using ProjectBackend.Models.ReleatedToSocial;

namespace ProjectBackend.Models.RelatedToRecommendation;

public class MovieUserPreference
{
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    public Dictionary<int, double> GenreWeights { get; set; } = new();
    public Dictionary<int, double> YearBuckets { get; set; } = new();
    public Dictionary<int, double> TmdbRatingBuckets { get; set; } = new();
}
