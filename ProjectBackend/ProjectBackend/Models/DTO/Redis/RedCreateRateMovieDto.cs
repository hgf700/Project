using ProjectBackend.Models.Redis;

namespace ProjectBackend.Models.DTO.Redis;

public class RedCreateObjectDto
{
    public string userId { get; set; }
    public string userNick { get; set; }
    public int objectId { get; set; }
    public ObjectType objectType { get; set; }
    public UserActionType UserCommittedAction { get; set; }
}
