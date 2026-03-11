using ProjectBackend.Models.DTO.Redis;
using ProjectBackend.Models.Redis;

namespace ProjectBackend.Services.Redis;

public interface INotificationsStore
{
    //Task<IEnumerable<FriendAction>> AllAsync();
    Task<IEnumerable<FriendAction>> GetUserNotificationsAsync(string UserId);
    Task MarkAsSeenAsync(string idOfAction);
    Task<IEnumerable<FriendAction>> GetUnseenAsync(string targetUserId);


}
