using ProjectBackend.Models;
using ProjectBackend.Services.interfaces;
using RestSharp;

namespace ProjectBackend.Services;

public class TmdbService : ITmdbService
{
    private readonly RestClient _client;
    private readonly string _apiToken;

    public TmdbService(IConfiguration configuration)
    {
        _apiToken = Environment.GetEnvironmentVariable("THE_MOVIE_DB_API")
                    ?? throw new Exception("TMDB API token not found");

        _client = new RestClient("https://api.themoviedb.org/3/");
    }

    public async Task<List<TMDB_Response>> GetPopularMoviesAsync(int page = 1)
    {
        var request = new RestRequest("discover/movie", Method.Get);

        request.AddHeader("Authorization", $"Bearer {_apiToken}");
        request.AddHeader("accept", "application/json");

        request.AddQueryParameter("include_video", "false");
        request.AddQueryParameter("language", "en-US");
        request.AddQueryParameter("sort_by", "popularity.desc");
        request.AddQueryParameter("page", page.ToString());

        var response = await _client.GetAsync<TmdbDiscoverResponse>(request);

        return response?.Results ?? new List<TMDB_Response>();
    }
}
