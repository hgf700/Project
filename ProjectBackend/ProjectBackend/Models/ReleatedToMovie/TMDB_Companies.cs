using System.Text.Json.Serialization;

namespace ProjectBackend.Models.ReleatedToMovie;

public class TMDB_Companies
{
    [JsonPropertyName("production_companies")]
    public List<ProductionCompaniesDto> ProductionCompanies { get; set; } = new();
}
public class ProductionCompaniesDto
{
    [JsonPropertyName("id")]
    public int CompanyId { get; set; }

    [JsonPropertyName("name")]
    public string CompanyName { get; set; }
}