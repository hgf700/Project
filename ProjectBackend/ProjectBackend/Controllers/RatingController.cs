using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProjectBackend.Controllers;

[Authorize]
[ApiController]
[Route("rating")]
public class RatingController : ControllerBase
{
    [Authorize]
    [HttpPost("rate-movie")]
    public IActionResult RateMovie(int MovieId, string RatingLabel)
    {


        return Challenge();
    }
}
