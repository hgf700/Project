using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectBackend.DB;
using ProjectBackend.Models.DTO.Redis;
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
            UserNick=dto.userNick,
            UserCommittedAction = dto.UserCommittedAction,
            ObjectId=dto.objectId,
            ObjectType = dto.objectType,
            CreatedDate = DateTime.UtcNow,
            Seen = false
        };

        var json = JsonSerializer.Serialize(notification);
        var key = $"notification:{notificationId}";

        await _redis.StringSetAsync(key, json, TimeSpan.FromMinutes(15));

        var friends = await _context.Friends
            .Where(f => f.UserId == dto.userId)
            .Select(f => f.FriendId)
            .ToListAsync();

        var tasks = new List<Task>();

        foreach (var friendId in friends)
        {
            var listKey = $"user_notifications:{friendId}";

            tasks.Add(_redis.ListLeftPushAsync(listKey, notificationId));
            tasks.Add(_redis.ListTrimAsync(listKey, 0, 99));
        }

        await Task.WhenAll(tasks);

        return notification;
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

}