using ProjectBackend.Models.ReleatedToMovie;
using RestSharp;

namespace ProjectBackend.Services.Tmdb;
public class TmdbSaveProductionCompaniesService
{
    private readonly RestClient _client;
    private readonly string _apiToken;

    public TmdbSaveProductionCompaniesService(IConfiguration configuration)
    {
        _apiToken = Environment.GetEnvironmentVariable("THE_MOVIE_DB_API")
                    ?? throw new Exception("TMDB API token not found");

        _client = new RestClient("https://api.themoviedb.org/3/");
    }

    public async Task<List<ProductionCompaniesDto>> GetProductionCompaniesAsync(int movie_id)
    {
        var request = new RestRequest($"movie/{movie_id}", Method.Get);

        request.AddHeader("Authorization", $"Bearer {_apiToken}");
        request.AddHeader("accept", "application/json");

        request.AddQueryParameter("movie_id", movie_id);
        request.AddQueryParameter("language", "en-US");

        var response = await _client.GetAsync<TMDB_Companies>(request);

        return response?.ProductionCompanies ?? new List<ProductionCompaniesDto>();
    }
}
