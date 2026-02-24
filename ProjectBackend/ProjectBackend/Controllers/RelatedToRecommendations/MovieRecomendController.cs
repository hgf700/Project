using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectBackend.DB;
using ProjectBackend.Models.DTO.RelatedToMovies;
using ProjectBackend.Models.DTO.RelatedToRecommendations;
using ProjectBackend.Models.ReleatedToSocial;
using ProjectBackend.Services;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace ProjectBackend.Controllers.RelatedToRecommendations;

[Authorize]
[ApiController]
[Route("recommendations")]
public class MovieRecomendController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public MovieRecomendController(
        ApplicationDbContext context
        )
    {
        _context = context;
    }

    [HttpPost("start-recomend-process-asp")]
    public async Task<IActionResult> StartRecommendAsp()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var likedMovies = await _context.UserMediaStatuses
            .Where(x => x.UserId == userId && x.Rating == RatingValue.Good)
            .Select(x => x.MovieId)
            .ToListAsync();

        var httpClient = new HttpClient();

        foreach (var movie in likedMovies)
        {
            var payload = new postMovieIdDto
            {
                MovieIds = likedMovies,
            };

            var json = JsonSerializer.Serialize(payload);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(
                "http://localhost:5000/recommend/start-recommend-process-py",
                content
            );

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode);
        }
        return Ok("Recommendation process started");
    }

    //[Authorize]
    //[HttpGet("show-recommendations")]
    //public async Task<IActionResult> ShowRecommendations()
    //{
    //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    //    if (userId == null) return Unauthorized();

    //    var userPreference = await _context.MovieUserPreferences
    //        .FirstOrDefaultAsync(x => x.UserId == userId);

    //    if (userPreference == null)
    //        return Ok(new List<getMovieRecommendationsDto>());

    //    var movies = await _context.Movies
    //        .Include(m => m.MovieGenre)
    //        .Include(m => m.MoviePeopleRole)
    //            .ThenInclude(ma => ma.PeopleRole)
    //        .ToListAsync();

    //    var scoredMovies = await Task.WhenAll(
    //        movies.Select(async m => new
    //        {
    //            Movie = m,
    //            Score = await _userMoviePreference.CalculateMovieScore(userPreference, m)
    //        })
    //    );

    //    var ranked = scoredMovies
    //        .OrderByDescending(x => x.Score)
    //        .Take(5)
    //        .Select(x => new getMovieRecommendationsDto
    //        {
    //            TmdbId = x.Movie.TmdbId,
    //            Title = x.Movie.Title,
    //            Overview = x.Movie.Overview,
    //            PosterPath = x.Movie.PosterPath,
    //            MovieRecommendations = x.Score
    //        })
    //        .ToList();

    //    var movies = await _context.Movies
    //.Include(m => m.RecomendTagMovies)
    //    .ThenInclude(rt => rt.RecomendTag)
    //.Select(m => new
    //{
    //    m.Id,
    //    m.Title,
    //    Tag = m.RecomendTagMovies
    //        .Select(rt => rt.RecomendTag.Tag)
    //        .FirstOrDefault()
    //})
    //.ToListAsync();

    //    return Ok(ranked);
    //}
}
