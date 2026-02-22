namespace ProjectBackend.Models.RelatedToRecommendation;

public class RecomendTag
{
    public int Id { get; set; } 
    public string Tag { get; set; } 
    public ICollection<RecomendTagMovie> RecomendTagMovie { get; set; } = new List<RecomendTagMovie>();
}
