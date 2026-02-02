using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectBackend.DB;
using ProjectBackend.Models.DTO;
using ProjectBackend.Models.DTO.GET;
using ProjectBackend.Models.DTO.POST;
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

    [Authorize]
    [HttpGet("users/{targetUserId}/view-profile-message")]
    public async Task<IActionResult> GetProfileMessages(string targetUserId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        if (userId == targetUserId)
            return BadRequest("Nie możesz napisać komentarza do samego siebie.");

        var targetUserExists = await _userManager.FindByIdAsync(targetUserId);

        if (targetUserExists == null)
            return NotFound("Użytkownik nie istnieje.");

        var comments = await _context.UserComments
            .Where(c => c.TargetUserId == targetUserId)
            .Select(c => new ProfileMessageGetDto
            {
                Id=c.Id,
                AuthorId = c.User.Id,        // autor komentarza
                AuthorEmail = c.User.Email,  // autor komentarza
                CreatedAt = c.CreatedAt,
                Text = c.Text
            })
            .ToListAsync();

        return Ok(comments);
    }

    [Authorize]
    [HttpPost("{targetUserId}/write-profile-message")]
    public async Task<IActionResult> WriteProfileMessage(string targetUserId, [FromBody] ProfileMessagePostDto dto)
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
            UserId = userId,           
            TargetUserId = targetUserId,
            Text = dto.Text,
            CreatedAt = DateTime.UtcNow,
        };


        _context.Add(comment);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [Authorize]
    [HttpDelete("delete-profile-message/{messageId}")]
    public async Task<IActionResult> DeleteProfileMessage(int messageId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var comment = await _context.UserComments
            .FirstOrDefaultAsync(c => c.Id == messageId);

        if (comment == null)
            return NotFound();

        // 🔐 tylko autor LUB właściciel profilu
        if (comment.UserId != userId && comment.TargetUserId != userId)
            return Forbid();

        _context.UserComments.Remove(comment);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    //niespraw

    [Authorize]
    [HttpPost("follow-profile/{targetUserId}")]
    public async Task<IActionResult> FollowUserProfile(string targetUserId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        if (userId == targetUserId)
            return BadRequest("Nie możesz like account do samego siebie.");

        var targetUserExists = await _userManager.FindByIdAsync(targetUserId);

        if (targetUserExists == null)
            return NotFound("Użytkownik nie istnieje.");

        var follow = new UserFollow
        {
            UserId = userId,
            TargetUserId = targetUserId,
            CreatedAt = DateTime.UtcNow,
        };


        _context.Add(follow);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [Authorize]
    [HttpDelete("stop-follow-profile/{targetUserId}")]
    public async Task<IActionResult> StopFollowUserProfile(string targetUserId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        if (userId == targetUserId)
            return BadRequest("Nie możesz like account do samego siebie.");

        var targetUserExists = await _userManager.FindByIdAsync(targetUserId);

        if (targetUserExists == null)
            return NotFound("Użytkownik nie istnieje.");

        var follow = await _context.UserFollows
            .FirstOrDefaultAsync(f => f.UserId == userId && f.TargetUserId == targetUserId);

        if (follow == null)
            return NotFound("Nie jesteś obserwatorem tego użytkownika.");

        _context.UserFollows.Remove(follow);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [Authorize]
    [HttpGet("view-follows-of-profile")]
    public async Task<IActionResult> GetProfileFollows()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var follows = await _context.UserFollows
        .Where(f => f.UserId == userId)
        .Select(f => new FollowedProfilesDto
        {
            UserId = f.UserId,                 
            TargetUserId = f.TargetUserId,     
            TargetUserEmail = f.TargetUser.Email 
        })
        .ToListAsync();

        return Ok(follows);
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
    [HttpDelete("like-playlist/{playlistId}")]
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


}
