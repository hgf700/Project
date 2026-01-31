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
    [HttpPost("share-playlist")]
    public async Task<IActionResult> SharePlaylist([FromBody] SharePlaylistIdDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var exists = await _context.Playlists
            .AnyAsync(p => p.UserId == userId && p.Name == dto.Name);

        

        var playlist = new Playlist
        {
            Name = dto.Name,
            UserId = userId
        };
        _context.Playlists.Add(playlist);
        await _context.SaveChangesAsync();

        return Ok(new { playlist.Id, playlist.Name });
    }
}
