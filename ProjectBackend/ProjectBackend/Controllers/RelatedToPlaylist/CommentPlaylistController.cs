using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using ProjectBackend.DB;
using ProjectBackend.Models.DTO.RelatedToPlaylist;
using ProjectBackend.Models.ReleatedToSocial;
using ProjectBackend.Services.Redis;
using StackExchange.Redis;
using System.Security.Claims;

namespace ProjectBackend.Controllers.RelatedToPlaylist;

[EnableRateLimiting("ratelimit")]
[Authorize]
[ApiController]
[Route("playlist-message")]
public class CommentPlaylistController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly INotificationsStore _redis;

    // ciezko redis narazie tutaj bo to wymaga wiekszego przemyslenia najlepiej moja playlista / do ktorej dodany jestem
    public CommentPlaylistController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        INotificationsStore redis
        )
    {
        _redis=redis;
        _context = context;
        _userManager = userManager;
    }

    [Authorize]
    [HttpPost("comment-playlist/{playlistId}")]
    public async Task<IActionResult> CommentPlaylist(int playlistId, [FromBody] postCommentPlaylistDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var playlist = await _context.Playlists
            .AnyAsync(p => p.Id == playlistId);

        if (playlist == null)
            return NotFound("Playlist not found");

        var comment = new PlaylistComment
        {
            PlaylistId = playlistId,
            UserId = userId,
            Text = dto.Text,
            CreatedAt= DateTime.UtcNow,
        };

        _context.PlaylistComments.Add(comment);

        await _context.SaveChangesAsync();



        return Ok();
    }

    [Authorize]
    [HttpDelete("delete-comment-playlist/{playlistId}/{commentId}")]
    public async Task<IActionResult> DeleteCommentPlaylist(int playlistId,int commentId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var playlist = await _context.Playlists
            .AnyAsync(p => p.Id == playlistId);

        if (playlist == null)
            return NotFound("Playlist not found");

        var playlistComment= await _context.PlaylistComments.FirstOrDefaultAsync(
            p => p.PlaylistId == playlistId && p.Id== commentId);

        if (playlistComment == null)
            return NotFound("Comment not found");

        // Sprawdzenie właściciela komentarza
        if (playlistComment.UserId != userId)
            return Unauthorized("You are not allowed to delete this comment");

        _context.PlaylistComments.Remove(playlistComment);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [Authorize]
    [HttpGet("view-comments-playlist/{playlistId}")]
    public async Task<IActionResult> GetLikedPlaylist(int playlistId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var comments = await _context.PlaylistComments
            .Where(p => p.PlaylistId == playlistId)
            .Select(p => new getPlaylistCommentsDto
            {
                Text = p.Text,
                UserEmail = p.User.UserName,
                CreatedAt = p.CreatedAt,
            })
            .ToListAsync();

        return Ok(comments);
    }
}
