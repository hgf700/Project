using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectBackend.DB;
using ProjectBackend.Models.DTO.GET;
using ProjectBackend.Models.ReleatedToSocial;
using System.Security.Claims;

namespace ProjectBackend.Controllers.RelatedToUserProfile;

[Authorize]
[ApiController]
[Route("follow")]
public class FollowProfilesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public FollowProfilesController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
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
}
