using System.Text.Json.Serialization;

namespace ProjectBackend.Models.ReleatedToMovie;

public class TMDB_Credits_Response
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("cast")]
    public List<TMDB_Cast_Response> Casts { get; set; } = new();
}

public class TMDB_Cast_Response
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("original_name")]
    public string OriginalName { get; set; }

    [JsonPropertyName("profile_path")]
    public string ProfilePath { get; set; }

    [JsonPropertyName("character")]
    public string Character { get; set; }

    [JsonPropertyName("order")]
    public int Order { get; set; }

    [JsonPropertyName("known_for_department")]
    public string KnownFor { get; set; }
    
}
