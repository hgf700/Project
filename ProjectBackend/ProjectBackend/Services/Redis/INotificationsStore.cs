using ProjectBackend.Models.DTO.Redis;
using ProjectBackend.Models.Redis;

namespace ProjectBackend.Services.Redis;

public interface INotificationsStore
{
    Task<IEnumerable<FriendAction>> AllAsync();
    Task<IEnumerable<FriendAction>> SearchAsync(int? targetUserId, bool? seen);
    Task<FriendAction?> OneAsync(int idOfAction);
    Task<FriendAction> CreateAsync(CreateFriendActionDto dto);
    Task<FriendAction?> UpdateAsync(int idOfAction, UpdateFriendActionDto dto);
    Task DeleteAsync(int idOfAction);
    Task DeleteAllAsync();
}
