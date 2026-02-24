namespace ProjectBackend.Models.DTO.RelatedToMovies;

public class getTmdbPersonCreditDto
{
    public int TmdbId { get; set; }
    public string OriginalName { get; set; } = null!;
    public string? ProfilePath { get; set; }
    public double? Popularity { get; set; }
    public string? Character { get; set; }
    public int? Order { get; set; }
    public string? Department { get; set; }
    public string? Job { get; set; }
    public string? KnownFor { get; set; }
}
