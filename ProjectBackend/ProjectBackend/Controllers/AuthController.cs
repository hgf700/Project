using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ProjectBackend.DB;
using ProjectBackend.ExtraTools;
using ProjectBackend.Models.ReleatedToSocial;
using ProjectBackend.Services;
using Sprache;
using System.Security.Claims;


namespace ProjectBackend.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AuthController> _logger;
    private readonly JwtService _jwtService;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext context,
        ILogger<AuthController> logger,
        JwtService jwtService
        )
    {
        _userManager = userManager;
        _context = context;
        _logger= logger;
        _jwtService = jwtService;

    }

    [HttpGet("signin-google")]
    public IActionResult SignInWithGoogle(string returnUrl = "/")
    {

        var redirectUrl = Url.Action(
            "GoogleResponse",
            "Auth",
            null,
            Request.Scheme
        );

        var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("google-response")]
    public async Task<IActionResult> GoogleResponse()
    {

        var authenticateResult = await HttpContext.AuthenticateAsync(
            GoogleDefaults.AuthenticationScheme);

        if (!authenticateResult.Principal.Identities.Any(i => i.AuthenticationType == "Google"))
            return Unauthorized();

        if (!authenticateResult.Succeeded)
            return Unauthorized();

        var principal = authenticateResult.Principal;

        var email = principal.FindFirstValue(ClaimTypes.Email);
        var googleId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        if (email == null)
            return BadRequest("Brak emaila z Google");

        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var createResult = await _userManager.CreateAsync(user);

            if (!createResult.Succeeded)
                return BadRequest(createResult.Errors);

            var loginInfo = new UserLoginInfo(
                "Google",
                googleId,
                "Google");

            await _userManager.AddLoginAsync(user, loginInfo);
        }

        var jwt = _jwtService.GenerateToken(user);
            
        return Redirect(
            $"http://localhost:4200/login-callback?token={jwt}"
        );
    }
}