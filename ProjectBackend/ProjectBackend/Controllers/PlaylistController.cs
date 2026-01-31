using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectBackend.DB;
using ProjectBackend.Models.DTO;
using ProjectBackend.Models.ReleatedToMovie;
using ProjectBackend.Models.ReleatedToPlaylist;
using ProjectBackend.Models.ReleatedToSocial;
using ProjectBackend.Services;
using ProjectBackend.Services.interfaces;
using System.Data;
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
    public async Task<IActionResult> CreatePlaylist([FromBody] CreatePlaylistNameDto dto)
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
            UserId = userId,
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

    [HttpPost("{playlistId}/movies/{tmdbId}")]
    public async Task<IActionResult> AddToPlaylist(int playlistId, int tmdbId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var playlist = await _context.Playlists
            .FirstOrDefaultAsync(p => p.Id == playlistId && p.UserId == userId);

        if (playlist == null) return NotFound("Playlist not found");

        bool canEdit = await _context.PlaylistMembers.AnyAsync(pm =>
            pm.PlaylistId == playlistId &&
            pm.UserId == userId &&
            (pm.Role == PlaylistRole.Owner || pm.Role == PlaylistRole.Editor)
        );

        if(!canEdit)
            return Unauthorized();

        var movie = await _context.Movies.SingleOrDefaultAsync(m => m.TmdbId == tmdbId);
        if (movie == null)
            return NotFound("movie not found");

        var exists = await _context.PlaylistValues.AnyAsync(pv =>
            pv.PlaylistId == playlistId &&
            pv.MovieId == movie.Id);

        if (exists) return BadRequest("Movie already in playlist");

        _context.PlaylistValues.Add(new PlaylistValue
        {
            PlaylistId = playlistId,
            MovieId = movie.Id
        });

        await _context.SaveChangesAsync();
        return Ok();
    }

    [Authorize]
    [HttpGet("show-playlists")]
    public async Task<IActionResult> ShowPlaylists()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var playlists = await _context.Playlists
            .Where(p =>
                p.UserId == userId ||
                _context.PlaylistMembers.Any(pm =>
                    pm.PlaylistId == p.Id && pm.UserId == userId)
            )
            .Select(p => new
            {
                p.Id,
                p.Name,
                IsOwner = p.UserId == userId
            })
            .ToListAsync();

        return Ok(playlists);
    }


    [Authorize]
    [HttpGet("show-playlist-values/{playlistId}")]
    public async Task<IActionResult> ShowPlaylistValues(int playlistId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var playlist = await _context.Playlists
            .Where(p => p.Id == playlistId && p.UserId == userId)
            .Select(p => new PlaylistDetailsDto
            {
                Id = p.Id,
                Name = p.Name,
                Movies = _context.PlaylistValues
                    .Where(pv => pv.PlaylistId == p.Id)
                    .Select(pv => new MovieDto
                    {
                        Id = pv.Movie.Id,
                        TmdbId = pv.Movie.TmdbId,
                        Title = pv.Movie.Title
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();

        if (playlist == null)
            return NotFound("Playlist not found");

        return Ok(playlist);
    }

    [Authorize]
    [HttpPost("{playlistId}/delete-from-playlist/{tmdbId}")]
    public async Task<IActionResult> DeleteFromPlaylist(int playlistId, int tmdbId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var playlist = await _context.Playlists
            .FirstOrDefaultAsync(p => p.Id == playlistId && p.UserId == userId);
        if (playlist == null) return NotFound("Playlist not found");

        bool canEdit = await _context.PlaylistMembers.AnyAsync(pm =>
            pm.PlaylistId == playlistId &&
            pm.UserId == userId &&
            (pm.Role == PlaylistRole.Owner || pm.Role == PlaylistRole.Editor)
        );

        if (!canEdit)
            return Unauthorized();

        var movie = await _context.Movies.SingleOrDefaultAsync(m => m.TmdbId == tmdbId);
        if (movie == null)
            return NotFound("movie not found");

        var exists = await _context.PlaylistValues.SingleOrDefaultAsync(pv =>
            pv.PlaylistId == playlistId &&
            pv.MovieId == movie.Id);

        if (exists == null)
            return NotFound("Movie not in playlist");

        _context.PlaylistValues.Remove(exists);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [Authorize]
    [HttpPost("delete-playlist")]
    public async Task<IActionResult> DeletePlaylist([FromBody] DeletePlaylistIdDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();

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

        _context.PlaylistValues.RemoveRange(playlistValues);
        _context.Playlists.Remove(playlist);

        await _context.SaveChangesAsync();

        return Ok();
    }

}
