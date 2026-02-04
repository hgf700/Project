using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectBackend.DB;
using ProjectBackend.Models.DTO;
using ProjectBackend.Models.DTO.RelatedToMovies;
using ProjectBackend.Models.ReleatedToPlaylist;
using ProjectBackend.Models.ReleatedToSocial;
using ProjectBackend.Services;
using System.Security.Claims;

namespace ProjectBackend.Controllers.RelatedToMovies;

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
    [HttpGet("show-user-rates")]
    public async Task<IActionResult> ShowPlaylistValues()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var rates = await _context.PlaylistLikes
            .Where(p =>p.UserId == userId)
            .Select(p => new getUserRatedMedia
            {
                PlaylistId= p.PlaylistId,
                UserId=p.UserId,
                CreatedAt= p.CreatedAt,
            })
            .FirstOrDefaultAsync();

        if (rates == null)
            return NotFound("Playlist not found");

        return Ok(rates);
    }

    [Authorize]
    [HttpPost("rate-movie")]
    public async Task<IActionResult> RateMovie(int movieId, [FromBody] postRateMoviePostDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var movie = await _context.Movies
            .FirstOrDefaultAsync(m => m.TmdbId == movieId);

        if (movie == null)
            return NotFound("Movie not found");

        var entry = await _context.UserMediaStatuses
            .FirstOrDefaultAsync(x => x.UserId == userId && x.MovieId == movie.Id);

        if (entry == null)
        {
            entry = new UserMediaStatus
            {
                UserId = userId,
                MovieId = movie.Id,   // 🔥 DB ID        
                Rating = dto.Rating 
            };
            _context.UserMediaStatuses.Add(entry);
        }
        else
        {
            entry.Rating = dto.Rating;
        }

        await _context.SaveChangesAsync();
        return Ok();
    }

    [Authorize]
    [HttpDelete("remove-rate")]
    public async Task<IActionResult> RemoveRate([FromBody] postRemoveRateIdPostDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var entry = await _context.UserMediaStatuses
            .FirstOrDefaultAsync(x => x.UserId == userId && x.MovieId == dto.movieId);

        if (entry == null)  
            return NotFound();

        _context.UserMediaStatuses.Remove(entry);

        await _context.SaveChangesAsync();
        return Ok();
    }


}
