using ProjectBackend.Models.ReleatedToMovie;
using RestSharp;

namespace ProjectBackend.Services.Tmdb;
public class TmdbLoadPeopleRoleService
{
    private readonly RestClient _client;
    private readonly string _apiToken;
    public TmdbLoadPeopleRoleService(IConfiguration configuration)
    {
        _apiToken = Environment.GetEnvironmentVariable("THE_MOVIE_DB_API")
                    ?? throw new Exception("TMDB API token not found");

        _client = new RestClient("https://api.themoviedb.org/3/");
    }

    public async Task<List<PeopleRole>> GetTopActorsAsync(int movie_id)
    {
        var request = new RestRequest($"movie/{movie_id}/credits", Method.Get);

        request.AddHeader("Authorization", $"Bearer {_apiToken}");
        request.AddHeader("accept", "application/json");

        var response = await _client.ExecuteAsync<TMDB_Credits_Response>(request);

        if (!response.IsSuccessful || response.Data == null)
            return new List<PeopleRole>();

        var result = response.Data.Casts?
            .Where(c => c.KnownFor == "Acting")
            .OrderBy(c => c.Order)
            .Take(3)
            .Select(c => new PeopleRole
            {
                TmdbId = c.Id,
                OriginalName = c.OriginalName,
                Popularity = c.Popularity,
                KnownFor = KnownForDepartment.Acting
            })
            .ToList() ?? new List<PeopleRole>();

        var director = response.Data.Casts?
            .FirstOrDefault(c => c.KnownFor == "Directing" && c.Job == "Director");

        if (director != null)
        {
            result.Add(new PeopleRole
            {
                TmdbId = director.Id,
                OriginalName = director.OriginalName,
                Popularity = director.Popularity,
                KnownFor = KnownForDepartment.Directing,
                Job = director.Job
            });
        }

        return result;
    }
}