using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectBackend.DB;
using ProjectBackend.Models.DTO;
using ProjectBackend.Models.ReleatedToSocial;
using ProjectBackend.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ProjectBackend.Controllers;

[Authorize]
[ApiController]
[Route("friend")]
public class FriendController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<FriendController> _logger;

    public FriendController(
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext context,
        ILogger<FriendController> logger
        )
    {
        _userManager = userManager;
        _context = context;
        _logger = logger;
    }

    [Authorize]
    [HttpPost("add-friend")]
    public async Task<IActionResult> AddFriend([FromBody] AddFriendDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var UserEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        var currentUser = await _userManager.FindByEmailAsync(UserEmail);

        var friendUser = await _userManager.FindByEmailAsync(dto.Email);
        if (friendUser == null)
            return NotFound("Użytkownik nie istnieje");

        if (currentUser.Id == friendUser.Id)
            return BadRequest("Nie możesz dodać siebie");

        bool exists = await _context.Friends.AnyAsync(f =>
            f.UserId == currentUser.Id &&
            f.FriendId == friendUser.Id);

        if (exists)
            return BadRequest("Znajomy już dodany");

        var friend = new Friend
        {
            UserId = currentUser.Id,
            FriendId = friendUser.Id
        };

        _context.Friends.Add(friend);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpGet("show-friends")]
    public async Task<IActionResult> ShowFriends()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var UserEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        var currentUser = await _userManager.FindByEmailAsync(UserEmail);

        if (currentUser == null) return Unauthorized();

        var friends = await _context.Friends
            .Where(f => f.UserId == currentUser.Id)
            .Include(f => f.FriendUser) 
            .Select(f => new
            {
                f.FriendId,
                Email = f.FriendUser.Email 
            })
            .ToListAsync();

        return Ok(friends);
    }

    [Authorize]
    [HttpPost("delete-friend")]
    public async Task<IActionResult> DeleteFriend([FromBody] DeleteFriendDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();

        var friendship = await _context.Friends.FirstOrDefaultAsync(f =>
            f.UserId == userId &&
            f.FriendId == dto.FriendId);

        if (friendship == null)
            return NotFound("Znajomy nie istnieje");

        _context.Friends.Remove(friendship);
        await _context.SaveChangesAsync();

        return Ok();
    }

}
