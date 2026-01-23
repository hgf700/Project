using ProjectBackend.Models;
using ProjectBackend.Services.interfaces;
using RestSharp;

namespace ProjectBackend.Services;

public class TmdbService : ITmdbService
{
    private readonly RestClient _client;

    public TmdbService(IConfiguration configuration)
    {
        var options = new RestClientOptions("https://api.themoviedb.org/3/discover/movie?language=en-US&sort_by=popularity.desc");
        _client = new RestClient(options);
    }

    public async Task<TMDB_Response> GetMovieAsync(int movieId)
    {
        var THE_MOVIE_DB_API = Environment.GetEnvironmentVariable("THE_MOVIE_DB_API");

        var request = new RestRequest($"movie/{movieId}");
        request.AddHeader("Authorization", $"Bearer {THE_MOVIE_DB_API}");
        request.AddHeader("accept", "application/json");

        var response = await _client.GetAsync<TMDB_Response>(request);
        return response;
    }
}
