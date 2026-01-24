using ProjectBackend.Models;
using RestSharp;

public class SeedGenresService
{
    private readonly RestClient _client;
    private readonly string _apiToken;

    public SeedGenresService(IConfiguration configuration)
    {
        _apiToken = Environment.GetEnvironmentVariable("THE_MOVIE_DB_API")
            ?? throw new Exception("TMDB API token not found");

        _client = new RestClient("https://api.themoviedb.org/3/");
    }

    public async Task<List<TmdbGenreDto>> GetAllGenresAsync()
    {
        var request = new RestRequest("genre/movie/list", Method.Get);
        request.AddHeader("Authorization", $"Bearer {_apiToken}");
        request.AddHeader("accept", "application/json");
        request.AddQueryParameter("language", "en-US");

        var response = await _client.GetAsync<TmdbGenreListResponse>(request);
        return response?.Genres ?? new List<TmdbGenreDto>();
    }
}
