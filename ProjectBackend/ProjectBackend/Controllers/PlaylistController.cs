using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectBackend.DB;
using ProjectBackend.Models.DTO;
using ProjectBackend.Models.ReleatedToPlaylist;
using ProjectBackend.Models.ReleatedToSocial;
using ProjectBackend.Services;
using ProjectBackend.Services.interfaces;
using System.Security.Claims;

namespace ProjectBackend.Controllers;

[Authorize]
[ApiController]
[Route("playlist")]
public class PlaylistController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;


    public PlaylistController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [Authorize]
    [HttpPost("create-playlist")]
    public async Task<IActionResult> CreatePlaylist([FromBody] CreatePlaylistDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var exists = await _context.Playlists
            .AnyAsync(p => p.UserId == userId && p.Name == dto.Name);

        if (exists)
            return BadRequest("Playlist already exists");

        var playlist = new Playlist
        {
            Name = dto.Name,
            UserId = userId
        };
        _context.Playlists.Add(playlist);
        await _context.SaveChangesAsync();

        return Ok(new { playlist.Id, playlist.Name });
    }

    [Authorize]
    [HttpPost("{playlistId}/movies/{movieId}")]
    public async Task<IActionResult> AddToPlaylist(int playlistId, int movieId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var playlist = await _context.Playlists
            .FirstOrDefaultAsync(p => p.Id == playlistId && p.UserId == userId);

        //potem ze jak nie istnijee to dodac?
        if (playlist == null)
            return NotFound("Playlist not found");

        var exists = await _context.PlaylistValues.AnyAsync(pv =>
            pv.PlaylistId == playlistId &&
            pv.MovieId == movieId);

        if (exists)
            return BadRequest("Movie already in playlist");

        var NewpPlaylistValue = new PlaylistValue
        {
            PlaylistId = playlistId,
            MovieId = movieId
        };

        _context.PlaylistValues.Add(NewpPlaylistValue);

        await _context.SaveChangesAsync();

        return Ok();
    }

    [Authorize]
    [HttpGet("show-playlists")]
    public async Task<IActionResult> ShowPlaylists()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var playlistResult = await _context.Playlists
        .Where(p => p.UserId == userId)
        .Select(p => new {
            p.Id,
            p.Name
        })
        .ToListAsync();

        return Ok(playlistResult);
    }
}
