using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectBackend.DB;
using ProjectBackend.Models.DTO.RelatedToMovies;
using ProjectBackend.Services;
using System.Security.Claims;

namespace ProjectBackend.Controllers.RelatedToRecommendations;

[Authorize]
[ApiController]
[Route("recommendations")]
public class MovieRecomendController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserMoviePreferenceService _userMoviePreference;

    
    public MovieRecomendController(
        ApplicationDbContext context,
        UserMoviePreferenceService userMoviePreference
        )
    {
        _context= context;
        _userMoviePreference = userMoviePreference;
    }

    [Authorize]
    [HttpGet("show-recommendations")]
    public async Task<IActionResult> ShowRecommendations()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var userPreference = await _context.MovieUserPreferences
            .FirstOrDefaultAsync(x => x.UserId == userId);

        if (userPreference == null)
            return Ok(new List<getMovieRecommendationsDto>());

        var movies = await _context.Movies
            .Include(m => m.MovieGenres)
            .Include(m => m.MovieActors)
                .ThenInclude(ma => ma.Actor)
            .ToListAsync();

        var scoredMovies = await Task.WhenAll(
            movies.Select(async m => new
            {
                Movie = m,
                Score = await _userMoviePreference.CalculateMovieScore(userPreference, m)
            })
        );

        var ranked = scoredMovies
            .OrderByDescending(x => x.Score)
            .Take(5)
            .Select(x => new getMovieRecommendationsDto
            {
                TmdbId = x.Movie.TmdbId,
                Title = x.Movie.Title,
                Overview = x.Movie.Overview,
                PosterPath = x.Movie.PosterPath,
                MovieRecommendations = x.Score
            })
            .ToList();

        return Ok(ranked);
    }
}
