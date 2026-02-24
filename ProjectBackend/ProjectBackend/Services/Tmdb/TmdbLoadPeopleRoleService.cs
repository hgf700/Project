using ProjectBackend.Models.DTO.RelatedToMovies;
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

    public async Task<List<getTmdbPersonCreditDto>> GetTopPopularPeoplesAsync(int movie_id)
    {
        var request = new RestRequest($"movie/{movie_id}/credits", Method.Get);

        request.AddHeader("Authorization", $"Bearer {_apiToken}");
        request.AddHeader("accept", "application/json");

        var response = await _client.ExecuteAsync<TMDB_Credits_Response>(request);

        if (!response.IsSuccessful || response.Data == null)
            return new List<getTmdbPersonCreditDto>();

        var result = new List<getTmdbPersonCreditDto>();

        var actors = response.Data.Casts
            .OrderBy(c => c.Order)
            .Take(3);

        foreach (var actor in actors)
        {
            result.Add(new getTmdbPersonCreditDto
            {
                TmdbId = actor.Id,
                OriginalName = actor.OriginalName,
                Popularity = actor.Popularity,
                ProfilePath = actor.ProfilePath,
                Character = actor.Character,
                Order = actor.Order,
                Department = "Acting",
                Job = "Actor"
            });
        }

        var director = response.Data.Crew
            .FirstOrDefault(c => c.Job == "Director" && c.Department=="Directing");

        if (director != null)
        {
            result.Add(new getTmdbPersonCreditDto
            {
                TmdbId = director.Id,
                OriginalName = director.OriginalName,
                Popularity = director.Popularity,
                ProfilePath = director.ProfilePath,
                Department = director.Department,
                Job = director.Job
            });
        }

        return result;
    }
}