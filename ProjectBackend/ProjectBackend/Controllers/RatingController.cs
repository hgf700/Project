using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectBackend.DB;
using ProjectBackend.Models.DTO;
using ProjectBackend.Models.ReleatedToSocial;
using ProjectBackend.Services;
using System.Security.Claims;

namespace ProjectBackend.Controllers;

[Authorize]
[ApiController]
[Route("rating")]
public class RatingController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    public RatingController(
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext context
        )
    {
        _userManager = userManager;
        _context = context;
    }


    [Authorize]
    [HttpPost("rate-movie")]
    public async Task<IActionResult> RateMovieAsync(int movieId, [FromBody] RateMovieDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var movie = await _context.Movies
            .FirstOrDefaultAsync(m => m.TmdbId == movieId);

        if (movie == null)
            return NotFound("Movie not found");

        var entry = await _context.UserMedias
            .FirstOrDefaultAsync(x => x.UserId == userId && x.MovieId == movie.Id);

        if (entry == null)
        {
            entry = new UserMedia
            {
                UserId = userId,
                MovieId = movie.Id,   // 🔥 DB ID
                Rating = dto.Rating
            };
            _context.UserMedias.Add(entry);
        }
        else
        {
            entry.Rating = dto.Rating;
        }

        await _context.SaveChangesAsync();
        return Ok();
    }

}
