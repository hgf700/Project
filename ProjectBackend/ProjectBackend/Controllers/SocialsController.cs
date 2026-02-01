using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectBackend.DB;
using ProjectBackend.Models.DTO;
using ProjectBackend.Models.ReleatedToPlaylist;
using ProjectBackend.Models.ReleatedToSocial;
using System.Security.Claims;

namespace ProjectBackend.Controllers;

[Authorize]
[ApiController]
[Route("social")]
public class SocialsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public SocialsController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [Authorize]
    [HttpPost("share-playlist/{playlistId}/members")]
    public async Task<IActionResult> SharePlaylist(int playlistId, [FromBody] SharePlaylistIdDto dto)
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
            PlaylistId= playlistId,
            UserId = dto.friendId,
            Role= PlaylistRole.Editor,
        };

        _context.PlaylistMembers.Add(playlistMember);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [Authorize]
    [HttpPut("change-to-public/{playlistId}")]
    public async Task<IActionResult> SharePublicPlaylist(int playlistId)
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

        if (playlist.IsPublic)
            return BadRequest("Playlist is already public");

        playlist.IsPublic = true;

        await _context.SaveChangesAsync();

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

        if (playlist.IsPublic==false)
            return BadRequest("Playlist is already private");

        playlist.IsPublic = false;

        await _context.SaveChangesAsync();

        return Ok(playlist);
    }

    [HttpGet("show-friends")]
    public async Task<IActionResult> ShowFriends()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var UserEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        var currentUser = await _userManager.FindByEmailAsync(UserEmail);

        if (currentUser == null) return Unauthorized();

        return Ok();
    }

    [Authorize]
    [HttpPost("write-profile-message")]
    public async Task<IActionResult> WriteProfileMessage(string targetUserId, [FromBody] ProfileMessageDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        if (userId == targetUserId)
            return BadRequest("Nie możesz napisać komentarza do samego siebie.");

        var targetUserExists = await _userManager.FindByIdAsync(targetUserId);

        if (targetUserExists==null)
            return NotFound("Użytkownik nie istnieje.");

        var comment = new UserComment
        {
            Text = dto.Text,
            UserId = userId,
            TargetUserId = targetUserId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Add(comment);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [Authorize]
    [HttpPost("delete-profile-message")]
    public async Task<IActionResult> DeleteProfileMessage(string targetUserId, [FromBody] ProfileMessageDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();



        return Ok();
    }

}
