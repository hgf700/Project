using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectBackend.DB;
using ProjectBackend.Models.DTO;
using ProjectBackend.Models.DTO.Redis;
using ProjectBackend.Models.DTO.RelatedToPlaylist;
using ProjectBackend.Models.Redis;
using ProjectBackend.Models.ReleatedToPlaylist;
using ProjectBackend.Models.ReleatedToSocial;
using ProjectBackend.Services.Redis;
using StackExchange.Redis;
using System.Security.Claims;

namespace ProjectBackend.Controllers.RelatedToPlaylist;


[Authorize]
[ApiController]
[Route("share-playlist-to-friend")]
public class SharePlaylistWithFriendsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly INotificationsStore _redis;

    public SharePlaylistWithFriendsController(
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
    [HttpPost("share-playlist/{playlistId}/members")]
    public async Task<IActionResult> SharePlaylist(int playlistId, [FromBody] SharePlaylistIdDto dto)
    {
        var useremail = User.FindFirstValue(ClaimTypes.Email);
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (useremail == null) return Unauthorized();
        if (userId == null) return Unauthorized();

        var playlist = await _context.Playlists
            .FirstOrDefaultAsync(p => p.Id == playlistId && p.UserId == userId);

        if (playlist == null) return NotFound("Playlist not found");

        var exists = await _context.Friends
            .AnyAsync(p => p.UserId == userId && p.FriendId == dto.friendId);

        if (!exists)
            return BadRequest("You can only share playlist with your friends");

        var alreadyMember = await _context.PlaylistMembers
            .AnyAsync(pm => pm.PlaylistId == playlistId && pm.UserId == dto.friendId);

        bool canEdit = await _context.PlaylistMembers.AnyAsync(pm =>
            pm.PlaylistId == playlistId &&
            pm.UserId == userId &&
            (pm.Role == PlaylistRole.Owner)
        );

        if (!canEdit)
            return Unauthorized();

        if (alreadyMember)
            return BadRequest("User is already a member of this playlist");

        var playlistMember = new PlaylistMember
        {
            PlaylistId = playlistId,
            UserId = dto.friendId,
            Role = PlaylistRole.Editor,
        };

        _context.PlaylistMembers.Add(playlistMember);
        await _context.SaveChangesAsync();

        if (playlist.IsPublic)
        {
            try
            {
                _ = _redis.NotifyObjectAsync(new RedCreateObjectDto
                {
                    userId = userId,
                    userNick = useremail,
                    objectId = playlistId,
                    objectType = ObjectType.Playlist,
                    UserCommittedAction = UserActionType.RateRemoved,
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        return Ok();
    }

    [Authorize]
    [HttpPost("stop-share-playlist/{playlistId}/members")]
    public async Task<IActionResult> StopSharePlaylist(int playlistId, [FromBody] SharePlaylistIdDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var playlist = await _context.Playlists
            .FirstOrDefaultAsync(p => p.Id == playlistId && p.UserId == userId);

        if (playlist == null) return NotFound("Playlist not found");

        var exists = await _context.Friends
            .AnyAsync(p => p.UserId == userId && p.FriendId == dto.friendId);

        if (!exists)
            return BadRequest("You can only share playlist with your friends");

        bool canEdit = await _context.PlaylistMembers.AnyAsync(pm =>
            pm.PlaylistId == playlistId &&
            pm.UserId == userId &&
            (pm.Role == PlaylistRole.Owner)
        );

        if (!canEdit)
            return Unauthorized();

        var removePlaylistMember= await _context.PlaylistMembers.FirstOrDefaultAsync(
            p=>p.PlaylistId == playlistId && p.UserId==dto.friendId);

        if (removePlaylistMember == null)
            return NotFound("Member not found");

        _context.PlaylistMembers.Remove(removePlaylistMember);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [Authorize]
    [HttpGet("show-playlist-friends/{playlistId}")]
    public async Task<IActionResult> ShowPlaylistValues(int playlistId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var playlistFreinds = await _context.PlaylistMembers.Where(p=>
            p.PlaylistId==playlistId && p.UserId != userId)
            .Select(pm => new getPlaylistFriendsDto
            {
                PlaylistId = pm.PlaylistId,
                UserId = pm.UserId,
                Email= pm.User.Email,
                Role = pm.Role,
            }).ToListAsync();

        if (playlistFreinds == null)
            return NotFound("Playlist not found");

        return Ok(playlistFreinds);
    }
}
