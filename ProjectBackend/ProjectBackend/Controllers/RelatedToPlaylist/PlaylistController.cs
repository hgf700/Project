using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using ProjectBackend.DB;
using ProjectBackend.Models.DTO;
using ProjectBackend.Models.DTO.Redis;
using ProjectBackend.Models.DTO.RelatedToPlaylist;
using ProjectBackend.Models.Redis;
using ProjectBackend.Models.ReleatedToMovie;
using ProjectBackend.Models.ReleatedToPlaylist;
using ProjectBackend.Models.ReleatedToSocial;
using ProjectBackend.Services;
using ProjectBackend.Services.interfaces;
using ProjectBackend.Services.Redis;
using StackExchange.Redis;
using System.Data;
using System.Security.Claims;

namespace ProjectBackend.Controllers.RelatedToPlaylist;

[EnableRateLimiting("ratelimit")]
[Authorize]
[ApiController]
[Route("playlist")]
public class PlaylistController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly INotificationsStore _redis;

    public PlaylistController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        INotificationsStore redis
        )
    {
        _context = context;
        _userManager = userManager;
        _redis= redis;
    }

    [Authorize]
    [HttpPost("create-playlist")]
    public async Task<IActionResult> CreatePlaylist([FromBody] postCreatePlaylistNamePostDto dto)
    {
        var useremail = User.FindFirstValue(ClaimTypes.Email);
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (useremail == null) return Unauthorized();
        if (userId == null) return Unauthorized();

        var exists = await _context.Playlists
            .AnyAsync(p => p.UserId == userId && p.Name == dto.Name);

        if (exists)
            return BadRequest("Playlist already exists");

        var playlist = new Playlist
        {
            Name = dto.Name,
            UserId = userId,
            IsPublic=false,
        };

        _context.Playlists.Add(playlist);
        await _context.SaveChangesAsync();

        var playlistMember = new PlaylistMember
        {
            PlaylistId = playlist.Id,
            UserId = userId,
            Role = PlaylistRole.Owner,
        };
        _context.PlaylistMembers.Add(playlistMember);

        await _context.SaveChangesAsync();

        return Ok(new { playlist.Id, playlist.Name });
    }

    //dto dodac jak playlist values i poprawic zapytanie bo nie optymalne strasznie opcja na dodawanie do playlisty
    // nie dziala nie wyswietlaja sie przyciski do dodania do playlisty itd

    [Authorize]
    [HttpGet("show-playlists")]
    public async Task<IActionResult> ShowPlaylists()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var playlists = await _context.Playlists
            .Where(p =>
                p.IsPublic ||
                _context.PlaylistMembers.Any(pm =>
                    pm.PlaylistId == p.Id && pm.UserId == userId)
            )
            .Select(p => new
            {
                p.Id,
                p.Name,
                Role = _context.PlaylistMembers
                    .Where(pm => pm.PlaylistId == p.Id && pm.UserId == userId)
                    .Select(pm => (PlaylistRole?)pm.Role)
                    .FirstOrDefault() ?? PlaylistRole.Viewer
            })
            .ToListAsync();

        return Ok(playlists);
    }

    [Authorize]
    [HttpPost("delete-playlist")]
    public async Task<IActionResult> DeletePlaylist([FromBody] postDeletePlaylistIdPostDto dto)
    {
        var useremail = User.FindFirstValue(ClaimTypes.Email);
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (useremail == null) return Unauthorized();
        if (userId == null) return Unauthorized();

        var playlist = await _context.Playlists
            .FirstOrDefaultAsync(p => p.Id == dto.PlaylistId && p.UserId == userId);

        if (playlist == null)
            return NotFound("Playlist nie istnieje");

        bool canEdit = await _context.PlaylistMembers.AnyAsync(pm =>
            pm.PlaylistId == dto.PlaylistId &&
            pm.UserId == userId &&
            (pm.Role == PlaylistRole.Owner)
        );

        if (!canEdit)
            return Unauthorized();

        var playlistValues = await _context.PlaylistValues
            .Where(pv => pv.PlaylistId == dto.PlaylistId)
            .ToListAsync();

        var playlistMembers = await _context.PlaylistMembers
            .Where(pv => pv.PlaylistId == dto.PlaylistId)
            .ToListAsync();

        _context.PlaylistMembers.RemoveRange(playlistMembers);
        _context.PlaylistValues.RemoveRange(playlistValues);
        _context.Playlists.Remove(playlist);

        await _context.SaveChangesAsync();

        return Ok();
    }
}