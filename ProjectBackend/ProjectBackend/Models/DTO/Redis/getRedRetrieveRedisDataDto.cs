using ProjectBackend.Models.Redis;

namespace ProjectBackend.Models.DTO.Rediss;

public class getRedRetrieveRedisDataDto
{
    public string userNick { get; set; }
    public UserActionType userCommittedAction { get; set; }
    public int? objectId { get; set; }
    public ObjectType? objectType { get; set; }
    public DateTime createdDate { get; set; }
}
