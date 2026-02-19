using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectBackend.DB;
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

        var movies = await _context.Movies
            .Include(m => m.MovieGenres)
            //.Include(m => m.MovieActors)
            .ToListAsync();

        var ranked = movies
            .Select(m => new
            {
                Movie = m,
                Score = _userMoviePreference.CalculateMovieScore(userPreference, m)
            })
            .OrderByDescending(x => x.Score)
            .Take(5)
            .Select(x => x.Movie);

        return Ok(ranked);
    }
}
