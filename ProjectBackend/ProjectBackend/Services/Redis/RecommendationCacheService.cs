using StackExchange.Redis;

namespace ProjectBackend.Services.Redis;

public class RecommendationCacheService
{
    private readonly IDatabase _db;

    public RecommendationCacheService(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }

    public async Task<string?> GetRecommendationAsync(string key)
    {
        return await _db.StringGetAsync(key);
    }

    public async Task SetRecommendationAsync(string key, string value)
    {
        await _db.StringSetAsync(key, value, TimeSpan.FromMinutes(10));
    }
}
