using System.Text.Json;
using StackExchange.Redis;
using ProjectBackend.Models.Redis;
using ProjectBackend.Models.DTO.Redis;

namespace ProjectBackend.Services.Redis;

public class NotificationsStore : INotificationsStore
{
    private readonly IDatabase _redis;

    public NotificationsStore(IConnectionMultiplexer muxer)
    {
        _redis = muxer.GetDatabase();
    }

    public async Task<FriendAction> CreateAsync(CreateFriendActionDto dto)
    {
        var id = Guid.NewGuid().ToString();

        var action = new FriendAction
        {
            IdOfAction = id,
            FriendNick = dto.FriendNick,
            FriendCommittedAction = dto.FriendCommittedAction,
            TargetUserId = dto.TargetUserId,
            ObjectId = dto.ObjectId,
            ObjectType = dto.ObjectType,
            CreatedDate = DateTime.UtcNow,
            Seen = false,
        };

        var key = $"notification:{id}";
        var json = JsonSerializer.Serialize(action);

        var listKey = $"user_notifications:{dto.TargetUserId}";

        await _redis.ListLeftPushAsync(listKey, id);
        await _redis.ListTrimAsync(listKey, 0, 99);

        await _redis.StringSetAsync(key, json, TimeSpan.FromHours(1));

        return action;
    }

    public async Task<IEnumerable<FriendAction>> GetUserNotificationsAsync(string targetUserId)
    {
        var ids = await _redis.ListRangeAsync($"user_notifications:{targetUserId}");

        if (ids.Length == 0)
            return Enumerable.Empty<FriendAction>();

        var keys = ids.Select(id => (RedisKey)$"notification:{id}").ToArray();
        var values = await _redis.StringGetAsync(keys);

        var result = new List<FriendAction>();

        foreach (var value in values)
        {
            if (!value.IsNullOrEmpty)
            {
                var action = JsonSerializer.Deserialize<FriendAction>((string)value);
                if (action != null)
                    result.Add(action);
            }
        }

        return result;
    }

    public async Task<int> CountUnseenAsync(string targetUserId)
    {
        var notifications = await GetUserNotificationsAsync(targetUserId);

        return notifications.Count(n => !n.Seen);
    }

    public async Task<IEnumerable<FriendAction>> GetUnseenAsync(string targetUserId)
    {
        var notifications = await GetUserNotificationsAsync(targetUserId);

        return notifications.Where(n => !n.Seen);
    }

    public async Task MarkAsSeenAsync(string idOfAction)
    {
        var key = $"notification:{idOfAction}";

        var value = await _redis.StringGetAsync(key);

        if (value.IsNullOrEmpty)
            return;

        var action = JsonSerializer.Deserialize<FriendAction>((string)value!);

        if (action == null)
            return;

        action.Seen = true;
        action.UpdatedDate = DateTime.UtcNow;

        var updatedJson = JsonSerializer.Serialize(action);

        await _redis.StringSetAsync(key, updatedJson);
    }
}