using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using ProjectBackend.DB;
using ProjectBackend.Models.DTO.Redis;
using ProjectBackend.Models.Redis;
using ProjectBackend.Models.ReleatedToPlaylist;
using ProjectBackend.Models.ReleatedToSocial;
using ProjectBackend.Services.Redis;
using StackExchange.Redis;
using System.Security.Claims;

namespace ProjectBackend.Controllers.RelatedToPlaylist;

[EnableRateLimiting("ratelimit")]
[Authorize]
[ApiController]
[Route("share-playlist-publically")]
public class SharePlaylistPublicallyController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly INotificationsStore _redis;

    public SharePlaylistPublicallyController(
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
    [HttpPut("change-to-public/{playlistId}")]
    public async Task<IActionResult> SharePublicPlaylist(int playlistId)
    {
        var useremail = User.FindFirstValue(ClaimTypes.Email);
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (useremail == null) return Unauthorized();
        if (userId == null) return Unauthorized();

        var playlist = await _context.Playlists
            .FirstOrDefaultAsync(p => p.Id == playlistId && p.UserId == userId);

        if (playlist == null) return NotFound("Playlist not found");

        bool canEdit = await _context.PlaylistMembers.AnyAsync(pm =>
            pm.PlaylistId == playlistId &&
            pm.UserId == userId &&
            (pm.Role == PlaylistRole.Owner)
        );

        if (!canEdit)
            return Unauthorized();

        if (playlist.IsPublic)
            return BadRequest("Playlist is already public");

        playlist.IsPublic = true;

        await _context.SaveChangesAsync();

        if (!playlist.IsPublic)
        {
            try
            {
                _ = _redis.NotifyObjectAsync(new RedCreateObjectDto
                {
                    userId = userId,
                    userNick = useremail,
                    objectId = playlistId,
                    objectType = ObjectType.Playlist,
                    UserCommittedAction = UserActionType.PlaylistMadePublic,
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        return Ok(playlist);
    }

    [Authorize]
    [HttpPut("change-to-private/{playlistId}")]
    public async Task<IActionResult> ChangePublicToPrivatePlaylist(int playlistId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var playlist = await _context.Playlists
            .FirstOrDefaultAsync(p => p.Id == playlistId && p.UserId == userId);

        if (playlist == null) return NotFound("Playlist not found");

        bool canEdit = await _context.PlaylistMembers.AnyAsync(pm =>
            pm.PlaylistId == playlistId &&
            pm.UserId == userId &&
            (pm.Role == PlaylistRole.Owner)
        );

        if (!canEdit)
            return Unauthorized();

        if (playlist.IsPublic == false)
            return BadRequest("Playlist is already private");

        playlist.IsPublic = false;

        await _context.SaveChangesAsync();

        return Ok(playlist);
    }
}
