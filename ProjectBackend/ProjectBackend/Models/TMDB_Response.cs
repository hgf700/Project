using System.Text.Json.Serialization;

namespace ProjectBackend.Models;

public class TMDB_Response
{
    [JsonPropertyName("backdrop_path")]
    public string BackdropPath { get; set; }  // string, np. "/8YFL5QQVPy3AgrEQxNYVSgiPEbe.jpg"

    [JsonPropertyName("original_title")]
    public string OriginalTitle { get; set; }

    [JsonPropertyName("overview")]
    public string Overview { get; set; }

    [JsonPropertyName("popularity")]
    public double Popularity { get; set; } // double lub float

    [JsonPropertyName("poster_path")]
    public string PosterPath { get; set; }

    [JsonPropertyName("release_date")]
    public string ReleaseDate { get; set; } // można string, potem parsować do DateTime jeśli trzeba

    [JsonPropertyName("vote_average")]
    public double VoteAverage { get; set; } // float lub double

    [JsonPropertyName("vote_count")]
    public int VoteCount { get; set; }

    [JsonPropertyName("adult")]
    public bool Adult { get; set; }

    [JsonPropertyName("genre_ids")]
    public int[] GenreIds { get; set; }
}
