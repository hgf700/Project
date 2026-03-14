using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectBackend.DB;
using ProjectBackend.Models.DTO.Redis;
using ProjectBackend.Models.DTO.Rediss;
using ProjectBackend.Models.Redis;
using ProjectBackend.Models.ReleatedToMovie;
using ProjectBackend.Models.ReleatedToSocial;
using StackExchange.Redis;
using System.Text.Json;

namespace ProjectBackend.Services.Redis;

public class NotificationsStore : INotificationsStore
{
    private readonly IDatabase _redis;
    private readonly ApplicationDbContext _context;

    public NotificationsStore(IConnectionMultiplexer muxer,
        ApplicationDbContext context
        )
    {
        _redis = muxer.GetDatabase();
        _context= context;
    }

    public async Task<UserAction> NotifyObjectAsync(RedCreateObjectDto dto)
    {
        var notificationId = Guid.NewGuid().ToString();

        var notification = new UserAction
        {
            IdOfAction = notificationId,
            UserId = dto.userId,
            UserNick = dto.userNick,
            UserCommittedAction = dto.UserCommittedAction,
            ObjectId = dto.objectId,
            ObjectType = dto.objectType,
            CreatedDate = DateTime.UtcNow,
            Seen = false
        };

        var jsonOptions = new JsonSerializerOptions
        {
            Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
        };
        var json = JsonSerializer.Serialize(notification, jsonOptions);
        var key = $"notification:{notificationId}";

        await _redis.StringSetAsync(key, json, TimeSpan.FromMinutes(15));

        var friends = await _context.Friends
            .Where(f => f.UserId == dto.userId)
            .Select(f => f.FriendId)
            .ToListAsync();

        // dodaj samego siebie, żeby własne akcje też były widoczne
        if (!friends.Contains(dto.userId))
            friends.Add(dto.userId);

        foreach (var friendId in friends)
        {
            var listKey = $"user_notifications:{friendId}";
            await _redis.ListLeftPushAsync(listKey, notificationId);
            await _redis.ListTrimAsync(listKey, 0, 99);
        }

        return notification;
    }

    public async Task<List<getRedRetrieveRedisDataDto>> RetrieveDataRedis(string userId)
    {
        var listKey = $"user_notifications:{userId}";

        var notificationIds = await _redis.ListRangeAsync(listKey, 0, 99);

        var notifications = new List<getRedRetrieveRedisDataDto>();

        foreach (var id in notificationIds)
        {
            var key = $"notification:{id}";
            var json = await _redis.StringGetAsync(key);

            if (json.IsNullOrEmpty)
                continue;

            var notification = JsonSerializer.Deserialize<UserAction>((string)json);

            if (notification == null)
                continue;

            var result = new getRedRetrieveRedisDataDto
            {
                userNick = notification.UserNick,
                userCommittedAction = notification.UserCommittedAction,
                objectId = notification.ObjectId,
                objectType = notification.ObjectType,
                createdDate = notification.CreatedDate
            };

            notifications.Add(result);
        }

        return notifications;
    }

    public async Task<TimeSpan> PingAsync()
    {
        var redisConnectionString = "localhost:6379";
        try
        {
            var redis = await ConnectionMultiplexer.ConnectAsync(redisConnectionString);
            var db = redis.GetDatabase();
            var pong = await db.PingAsync();
            await redis.CloseAsync();
            return pong;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd połączenia z Redis: {ex.Message}");
            return TimeSpan.Zero;
        }
    }

    public async Task<string> RetrieveLastNotification(string userId)
    {
        var listKey = $"user_notifications:{userId}";
        var lastNotificationId = await _redis.ListGetByIndexAsync(listKey, 0); // _redis jest typu IDatabase
        if (lastNotificationId.IsNullOrEmpty)
            return null;

        var json = await _redis.StringGetAsync($"notification:{lastNotificationId}");
        return json.IsNullOrEmpty ? null : json.ToString();
    }
}