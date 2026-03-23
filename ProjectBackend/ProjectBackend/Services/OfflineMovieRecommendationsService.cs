using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectBackend.DB;
using ProjectBackend.Models.DTO.RelatedToRecommendations;
using ProjectBackend.Models.ReleatedToSocial;
using StackExchange.Redis;
using System.Text;
using System.Text.Json;

namespace ProjectBackend.Services;

public class OfflineMovieRecommendationsService : BackgroundService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConnectionMultiplexer _redis;
    private readonly ILogger<OfflineMovieRecommendationsService> _logger;
    private readonly HttpClient _httpClient;

    public OfflineMovieRecommendationsService(
        ApplicationDbContext context,
        IConnectionMultiplexer redis,
        ILogger<OfflineMovieRecommendationsService> logger,
        UserManager<ApplicationUser> userManager,
        HttpClient httpClient // wstrzyknięty przez DI
    )
    {
        _context = context;
        _redis = redis;
        _logger = logger;
        _userManager = userManager;
        _httpClient = httpClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await GenerateRecommendationsForAllUsers(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas generowania rekomendacji offline");
            }

            await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
        }
    }

    private async Task GenerateRecommendationsForAllUsers(CancellationToken stoppingToken)
    {
        var allUserIds = await _userManager.Users
            .Select(u => u.Id)
            .ToListAsync(stoppingToken);

        var db = _redis.GetDatabase();

        foreach (var userId in allUserIds)
        {
            // pobranie polubionych filmów
            var likedMoviesIds = await _context.UserMediaStatuses
                .Where(x => x.UserId == userId && x.Rating == RatingValue.Good)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => x.MovieId)
                .Take(3)
                .ToListAsync(stoppingToken);

            List<string> tags;

            // jeśli mniej niż 3 polubienia → cold start, losowe tagi
            if (likedMoviesIds.Count < 3)
            {
                tags = await _context.RecomendTagMovies
                    .Select(rtm => rtm.RecomendTag.Tag)
                    .Distinct()
                    .OrderBy(x => Guid.NewGuid())
                    .Take(3)
                    .ToListAsync(stoppingToken);
            }
            else
            {
                tags = await _context.RecomendTagMovies
                    .Where(rtm => likedMoviesIds.Contains(rtm.MovieId))
                    .Select(rtm => rtm.RecomendTag.Tag)
                    .Distinct()
                    .ToListAsync(stoppingToken);
            }

            var payload = new postMovieTagDto { tags = tags };

            List<RecommendationDto> recommendations;
            try
            {
                recommendations = await CallMLService(payload);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Błąd wywołania ML dla userId={userId}");
                recommendations = new List<RecommendationDto>();
            }

            // zapis do Redis z TTL 24h
            var cacheKey = $"user:{userId}:recommendations";
            await db.StringSetAsync(
                cacheKey,
                JsonSerializer.Serialize(recommendations, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
                TimeSpan.FromHours(24)
            );
        }
    }

    private async Task<List<RecommendationDto>> CallMLService(postMovieTagDto payload)
    {
        var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(
            "http://localhost:5000/recommendations/receive-recommend-process-py",
            content
        );

        if (!response.IsSuccessStatusCode)
        {
            return new List<RecommendationDto>();
        }

        var result = await response.Content.ReadFromJsonAsync<getRecommendationsResponsePyDto>();

        if (result == null || result.recommendations == null)
            return new List<RecommendationDto>();

        // mapowanie na RecommendationDto z confidence = 0, bo FastAPI nie zwraca
        return result.recommendations.Select(r => new RecommendationDto
        {
            Title = r,
            Confidence = 0
        }).ToList();
    }
}

// DTO
public class RecommendationDto
{
    public string Title { get; set; } = string.Empty;
    public double Confidence { get; set; }
}