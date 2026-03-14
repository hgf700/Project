using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectBackend.DB;
using ProjectBackend.Models.DTO.Redis;
using ProjectBackend.Models.DTO.RelatedToPlaylist;
using ProjectBackend.Models.Redis;
using ProjectBackend.Models.ReleatedToPlaylist;
using ProjectBackend.Models.ReleatedToSocial;
using ProjectBackend.Services.Redis;
using System.Security.Claims;

namespace ProjectBackend.Controllers.RelatedToPlaylist;

[Authorize]
[ApiController]
[Route("like-playlist")]
public class LikePlaylistController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly INotificationsStore _redis;

    public LikePlaylistController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        INotificationsStore redis
        )
    {
        _context = context;
        _userManager = userManager;
        _redis= redis;
    }

    //chyba mam to nie dokonczone i nie sprawdzone / chyba widoku nie mam?

    [Authorize]
    [HttpPost("like-playlist/{playlistId}")]
    public async Task<IActionResult> LikePlaylist(int playlistId)
    {
        var useremail = User.FindFirstValue(ClaimTypes.Email);
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();
        if (useremail == null) return Unauthorized();

        var playlist = await _context.Playlists
            .AnyAsync(p => p.Id == playlistId);

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
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
        };

        _context.PlaylistLikes.Add(like);

        await _context.SaveChangesAsync();

        try
        {
            _ = _redis.NotifyObjectAsync(new RedCreateObjectDto
            {
                userId = userId,
                userNick = useremail,
                objectId = playlistId,
                objectType= ObjectType.Playlist,
                UserCommittedAction = UserActionType.PlaylistLiked,
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        
        return Ok();
    }

    [Authorize]
    [HttpDelete("stop-like-playlist/{playlistId}")]
    public async Task<IActionResult> StoplikePlaylist(int playlistId)
    {
        var useremail = User.FindFirstValue(ClaimTypes.Email);
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();
        if (useremail == null) return Unauthorized();

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

        try
        {
            _ = _redis.NotifyObjectAsync(new RedCreateObjectDto
            {
                userId = userId,
                userNick = useremail,
                objectId = playlistId,
                objectType = ObjectType.Playlist,
                UserCommittedAction = UserActionType.PlaylistUnliked,
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

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
            .Select(f => new getLikedPlaylistDto
            {
                UserId = f.UserId,
                PlaylistId = f.PlaylistId,
            })
            .ToListAsync();

        return Ok(likes);
    }
}
