using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using ProjectBackend.DB;
using ProjectBackend.Models.DTO.RelatedToUserProfile;
using ProjectBackend.Models.ReleatedToSocial;
using System.Security.Claims;

namespace ProjectBackend.Controllers.RelatedToUserProfile;

[EnableRateLimiting("ratelimit")]
[Authorize]
[ApiController]
[Route("profile-message")]
public class ProfileMessageController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ProfileMessageController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    //niesprawdzone tylko jako odbiorca redis?

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
            .Select(c => new getProfileMessageDto
            {
                Id = c.Id,
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
    public async Task<IActionResult> WriteProfileMessage(string targetUserId, [FromBody] postProfileMessagePostDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        if (userId == targetUserId)
            return BadRequest("Nie możesz napisać komentarza do samego siebie.");

        var targetUserExists = await _userManager.FindByIdAsync(targetUserId);

        if (targetUserExists == null)
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
}
