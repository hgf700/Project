using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectBackend.DB;
using ProjectBackend.Models.ReleatedToSocial;
using ProjectBackend.Services;

namespace ProjectBackend.Controllers;

[Authorize]
[ApiController]
[Route("rating")]
public class RatingController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    public RatingController(
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext context
        )
    {
        _userManager = userManager;
        _context = context;
    }

    [Authorize]
    [HttpPost("rate-movie")]
    public IActionResult RateMovie(int MovieId, int RatingValue)
    {


        return Challenge();
    }
}
