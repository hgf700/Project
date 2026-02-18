using ProjectBackend.Models.ReleatedToSocial;
using System.ComponentModel.DataAnnotations;

namespace ProjectBackend.Models.RelatedToRecommendation;

public class MovieUserPreference
{
    public MovieUserPreference()
    {
        GenreWeights = new Dictionary<int, double>();
        YearBuckets = new Dictionary<int, double>();
        TmdbRatingBuckets = new Dictionary<int, double>();
    }

    [Key]
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

    public Dictionary<int, double> GenreWeights { get; set; }
    public Dictionary<int, double> YearBuckets { get; set; }
    public Dictionary<int, double> TmdbRatingBuckets { get; set; }
}

