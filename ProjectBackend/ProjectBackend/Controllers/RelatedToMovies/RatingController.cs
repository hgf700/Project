using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectBackend.DB;
using ProjectBackend.Models.DTO;
using ProjectBackend.Models.DTO.Redis;
using ProjectBackend.Models.DTO.RelatedToMovies;
using ProjectBackend.Models.Redis;
using ProjectBackend.Models.RelatedToRecommendation;
using ProjectBackend.Models.ReleatedToPlaylist;
using ProjectBackend.Models.ReleatedToSocial;
using ProjectBackend.Services;
using ProjectBackend.Services.Redis;
using StackExchange.Redis;
using System.ComponentModel.Design;
using System.Security.Claims;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjectBackend.Controllers.RelatedToMovies;

[Authorize]
[ApiController]
[Route("rating")]
public class RatingController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly INotificationsStore _redis;
    public RatingController(
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext context,
        INotificationsStore redis
        )
    {
        _userManager = userManager;
        _context = context;
        _redis = redis;
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
        var ping = await _redis.PingAsync(); // ping jest typu TimeSpan
        Console.WriteLine($"Redis ping: {ping.TotalMilliseconds} ms");

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var user = await _userManager.FindByIdAsync(userId);

        var email = user.Email;

        var movie = await _context.Movies
            .Include(m => m.MovieGenre) // WAŻNE
            .FirstOrDefaultAsync(m => m.TmdbId == movieId);

        if (movie == null)
            return NotFound("Movie not found");

        var entry = await _context.UserMediaStatuses
            .FirstOrDefaultAsync(x => x.UserId == userId && x.MovieId == movie.Id);

        var userPreference = await _context.MovieUserPreferences
            .FirstOrDefaultAsync(x => x.UserId == userId);

        if (userPreference == null)
        {
            userPreference = new MovieUserPreference
            {
                UserId = userId
            };

            _context.MovieUserPreferences.Add(userPreference);
        }

        if (entry == null)
        {
            entry = new UserMediaStatus
            {
                UserId = userId,
                MovieId = movie.Id,
                Rating = dto.Rating
            };

            _context.UserMediaStatuses.Add(entry);

            var releaseYear = movie.ReleaseDate.Year;
        }
        else
        {
            var oldRating = entry.Rating;
        }
        await _context.SaveChangesAsync();

        await _redis.NotifyMovieRatedAsync(new RedCreateRateMovieDto
        {
            userId = userId,
            userNick = email,
            movieId = movie.Id,
            FriendCommittedAction = UserActionType.Rate
        });

        return Ok();
    }

    [Authorize]
    [HttpPost("remove-rate")]
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
