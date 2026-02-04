using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectBackend.DB;
using ProjectBackend.Models.DTO;
using ProjectBackend.Models.ReleatedToPlaylist;
using ProjectBackend.Models.ReleatedToSocial;
using System.Security.Claims;

namespace ProjectBackend.Controllers.RelatedToPlaylist;

[Authorize]
[ApiController]
[Route("playlist-value")]
public class PlaylistValuesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public PlaylistValuesController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [Authorize]
    [HttpPost("{playlistId}/movies/{tmdbId}")]
    public async Task<IActionResult> AddToPlaylist(int playlistId, int tmdbId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var playlist = await _context.Playlists
            .FirstOrDefaultAsync(p =>
                p.Id == playlistId &&
                (
                    p.UserId == userId ||
                    _context.PlaylistMembers.Any(pm =>
                        pm.PlaylistId == p.Id && pm.UserId == userId
                    )
                )
            );

        if (playlist == null)
            return NotFound("Playlist not found");

        bool canEdit =
            playlist.UserId == userId ||
            await _context.PlaylistMembers.AnyAsync(pm =>
                pm.PlaylistId == playlistId &&
                pm.UserId == userId &&
                pm.Role == PlaylistRole.Editor
            );

        if (!canEdit)
            return Forbid();

        var movie = await _context.Movies.SingleOrDefaultAsync(m => m.TmdbId == tmdbId);
        if (movie == null)
            return NotFound("movie not found");

        bool exists = await _context.PlaylistValues.AnyAsync(pv =>
            pv.PlaylistId == playlistId &&
            pv.MovieId == movie.Id);

        if (exists)
            return BadRequest("Movie already in playlist");

        _context.PlaylistValues.Add(new PlaylistValue
        {
            PlaylistId = playlistId,
            MovieId = movie.Id
        });

        await _context.SaveChangesAsync();
        return Ok();
    }

    [Authorize]
    [HttpGet("show-playlist-values/{playlistId}")]
    public async Task<IActionResult> ShowPlaylistValues(int playlistId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var playlist = await _context.Playlists
            .Where(p =>
                p.Id == playlistId &&
                (
                    p.UserId == userId ||
                    p.IsPublic ||
                    _context.PlaylistMembers.Any(pm =>
                        pm.PlaylistId == p.Id && pm.UserId == userId
                    )
                )
            )
            .Select(p => new PlaylistDetailsDto
            {
                Id = p.Id,
                Name = p.Name,
                Role = p.UserId == userId
                    ? PlaylistRole.Owner
                    : _context.PlaylistMembers
                        .Where(pm => pm.PlaylistId == p.Id && pm.UserId == userId)
                        .Select(pm => pm.Role)
                        .FirstOrDefault(),

                Movies = _context.PlaylistValues
                    .Select(pv => pv)
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
    [HttpDelete("{playlistId}/delete-from-playlist/{tmdbId}")]
    public async Task<IActionResult> DeleteFromPlaylist(int playlistId, int tmdbId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var playlist = await _context.Playlists
            .FirstOrDefaultAsync(p =>
                p.Id == playlistId &&
                (
                    p.UserId == userId ||
                    _context.PlaylistMembers.Any(pm =>
                        pm.PlaylistId == p.Id && pm.UserId == userId
                    )
                )
            );

        if (playlist == null)
            return NotFound("Playlist not found");

        bool canEdit =
            playlist.UserId == userId ||
            await _context.PlaylistMembers.AnyAsync(pm =>
                pm.PlaylistId == playlistId &&
                pm.UserId == userId &&
                pm.Role == PlaylistRole.Editor
            );

        if (!canEdit)
            return Forbid();

        var movie = await _context.Movies.SingleOrDefaultAsync(m => m.TmdbId == tmdbId);
        if (movie == null)
            return NotFound("movie not found");

        var playlistValue = await _context.PlaylistValues
            .SingleOrDefaultAsync(pv =>
                pv.PlaylistId == playlistId &&
                pv.MovieId == movie.Id
            );

        if (playlistValue == null)
            return NotFound("Movie not in playlist");

        _context.PlaylistValues.Remove(playlistValue);
        await _context.SaveChangesAsync();

        return NoContent();
    }

}
