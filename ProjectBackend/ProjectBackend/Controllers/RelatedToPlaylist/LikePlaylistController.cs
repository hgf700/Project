using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectBackend.DB;
using ProjectBackend.Models.DTO.GET;
using ProjectBackend.Models.ReleatedToPlaylist;
using ProjectBackend.Models.ReleatedToSocial;
using System.Security.Claims;

namespace ProjectBackend.Controllers.RelatedToPlaylist;

[Authorize]
[ApiController]
[Route("like-playlist")]
public class LikePlaylistController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public LikePlaylistController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [Authorize]
    [HttpPost("like-playlist/{playlistId}")]
    public async Task<IActionResult> LikePlaylist(int playlistId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var playlist = await _context.Playlists
            .FirstOrDefaultAsync(p => p.Id == playlistId);

        if (playlist == null)
            return NotFound("Playlist not found");

        // sprawdzamy, czy user już polubił
        var alreadyLiked = await _context.PlaylistLikes
            .AnyAsync(l => l.PlaylistId == playlistId && l.UserId == userId);

        if (alreadyLiked)
            return BadRequest("You already liked this playlist");

        var like = new PlaylistLike
        {
            PlaylistId = playlistId,
            UserId = userId
        };

        _context.PlaylistLikes.Add(like);

        // opcjonalnie aktualizujemy licznik w Playlist
        //playlist.Likes = await _context.PlaylistLikes.CountAsync(l => l.PlaylistId == playlistId);


        //var likesCount = await _context.PlaylistLikes.CountAsync(l => l.PlaylistId == playlistId);
        //var userLiked = await _context.PlaylistLikes.AnyAsync(l => l.PlaylistId == playlistId && l.UserId == currentUserId);



        await _context.SaveChangesAsync();

        return Ok();
    }

    [Authorize]
    [HttpDelete("stop-like-playlist/{playlistId}")]
    public async Task<IActionResult> UnlikePlaylist(int playlistId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var like = await _context.PlaylistLikes
            .FirstOrDefaultAsync(l => l.PlaylistId == playlistId && l.UserId == userId);

        if (like == null)
            return NotFound("You have not liked this playlist");

        _context.PlaylistLikes.Remove(like);

        // aktualizacja licznika
        var playlist = await _context.Playlists.FirstOrDefaultAsync(p => p.Id == playlistId);
        if (playlist != null)
        {
            //playlist.Likes = await _context.PlaylistLikes.CountAsync(l => l.PlaylistId == playlistId);
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [Authorize]
    [HttpGet("view-liked-playlist")]
    public async Task<IActionResult> GetLikedPlaylist()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var likes = await _context.PlaylistLikes
        .Where(f => f.UserId == userId)
        .Select(f => new viewLikedPlaylistDto
        {
            UserId = f.UserId,
            PlaylistId = f.PlaylistId,
        })
        .ToListAsync();

        return Ok(likes);
    }
}
