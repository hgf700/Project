using ProjectBackend.Models.DTO.Redis;
using ProjectBackend.Models.DTO.Rediss;
using ProjectBackend.Models.Redis;

namespace ProjectBackend.Services.Redis;

public interface INotificationsStore
{
    Task<UserAction> NotifyObjectAsync(RedCreateObjectDto dto);
    Task<List<getRedRetrieveRedisDataDto>> RetrieveDataRedis(string userId);

    //Task<IEnumerable<FriendAction>> AllAsync();
    //Task<IEnumerable<FriendAction>> GetUserNotificationsAsync(string UserId);
    //Task MarkAsSeenAsync(string idOfAction);
    //Task<IEnumerable<FriendAction>> GetUnseenAsync(string targetUserId);
    //Task<FriendAction> CreateAsync(CreateFriendActionDto dto);
}
