using ProjectBackend.Models.ReleatedToMovie;
using RestSharp;

namespace ProjectBackend.Services;
public class LoadActorsService
{
    private readonly RestClient _client;
    private readonly string _apiToken;
    public LoadActorsService(IConfiguration configuration)
    {
        _apiToken = Environment.GetEnvironmentVariable("THE_MOVIE_DB_API")
                    ?? throw new Exception("TMDB API token not found");

        _client = new RestClient("https://api.themoviedb.org/3/");
    }

    public async Task<List<TMDB_Cast_Response>> GetTopActorsAsync(int movie_id)
    {
        var request = new RestRequest($"movie/{movie_id}/credits", Method.Get);

        request.AddHeader("Authorization", $"Bearer {_apiToken}");
        request.AddHeader("accept", "application/json");

        var response = await _client.ExecuteAsync<TMDB_Credits_Response>(request);

        if (!response.IsSuccessful || response.Data?.Casts == null)
            return new List<TMDB_Cast_Response>();

        return response.Data.Casts
            .Where(c => c.KnownFor == "Acting")
            .OrderBy(c => c.Order)
            .Take(3)
            .ToList();
    }
}