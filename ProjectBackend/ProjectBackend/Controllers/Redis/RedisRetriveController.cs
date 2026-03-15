using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ProjectBackend.Models.DTO.RelatedToUserProfile;
using ProjectBackend.Services.Redis;
using System.Security.Claims;

namespace ProjectBackend.Controllers.Redis;

[Authorize]
[ApiController]
[Route("redis")]
[EnableRateLimiting("ratelimit")]
public class RedisRetriveController : ControllerBase
{
    private readonly INotificationsStore _redis;

    public RedisRetriveController(INotificationsStore redis
        )
    {
        _redis= redis;
    }

    [HttpGet("get-redis-data")]
    public async Task<IActionResult> GetRedisData()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var json = await _redis.RetrieveDataRedis(userId); // <-- wywołujesz metodę serwisu, nie Redis

        if (json == null)
            return Ok("Brak powiadomień");

        return Ok(json);
    }
}
