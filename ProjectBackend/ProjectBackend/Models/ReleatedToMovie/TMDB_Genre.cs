using System.Text.Json.Serialization;

namespace ProjectBackend.Models.ReleatedToMovie;

public class TmdbGenreListResponse
{
    [JsonPropertyName("genres")]
    public List<TmdbGenreDto> Genres { get; set; } = new();
}

public class TmdbGenreDto
{
    [JsonPropertyName("id")]
    public int TmdbId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}
