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
}
