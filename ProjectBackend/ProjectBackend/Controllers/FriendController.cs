using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectBackend.DB;
using ProjectBackend.DTO;
using ProjectBackend.Models;
using ProjectBackend.Services;
using System.ComponentModel.DataAnnotations;

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


    [HttpPost("add-friend")]
    public async Task<IActionResult> AddFriend([FromBody] AddFriendDto dto)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null)
            return Unauthorized();

        var friendUser = await _userManager.FindByEmailAsync(dto.Email);
        if (friendUser == null)
            return NotFound("Użytkownik nie istnieje");

        if (currentUser.Id == friendUser.Id)
            return BadRequest("Nie możesz dodać siebie");

        bool exists = await _context.Friends.AnyAsync(f =>
            f.UserId == currentUser.Id &&
            f.FriendUserId == friendUser.Id);

        if (exists)
            return BadRequest("Znajomy już dodany");

        var friend = new Friend
        {
            UserId = currentUser.Id,
            FriendUserId = friendUser.Id
        };

        _context.Friends.Add(friend);
        await _context.SaveChangesAsync();

        return Ok();
    }
}
