using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectBackend.DB;
using ProjectBackend.Models.DTO.RelatedToMovies;
using ProjectBackend.Models.DTO.RelatedToRecommendations;
using ProjectBackend.Models.ReleatedToSocial;
using ProjectBackend.Services;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace ProjectBackend.Controllers.RelatedToRecommendations;

[Authorize]
[ApiController]
[Route("recommendations")]
public class MovieRecomendController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public MovieRecomendController(
        ApplicationDbContext context
        )
    {
        _context = context;
    }

    [Authorize]
    [HttpPost("start-recommend-process-asp")]
    public async Task<IActionResult> StartRecommendAsp()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var likedMoviesIds = await _context.UserMediaStatuses
            .Where(x => x.UserId == userId && x.Rating == RatingValue.Good)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => x.MovieId)
            .Take(3)
            .ToListAsync();

        var tags = await _context.RecomendTagMovies
            .Where(rtm => likedMoviesIds.Contains(rtm.MovieId))
            .Select(rtm => rtm.RecomendTag.Tag)
            .Distinct()
            .ToListAsync();

        var httpClient = new HttpClient();

        var payload = new postMovieTagDto
        {
            tags = tags,
        };

        var json = JsonSerializer.Serialize(payload);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(
            "http://localhost:5000/recommendations/receive-recommend-process-py",
            content
        );

        if (!response.IsSuccessStatusCode)
               return StatusCode((int)response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<getRecommendationsResponsePyDto>();

        return Ok(result.recommendations);
    }
}
