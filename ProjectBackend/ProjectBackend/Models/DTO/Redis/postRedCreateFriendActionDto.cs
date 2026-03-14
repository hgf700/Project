using ProjectBackend.Models.Redis;

namespace ProjectBackend.Models.DTO.Redis;

public class postRedCreateFriendActionDto
{
    public string FriendNick {  get; set; }
    public string TargetUserId { get; set; }
    public int ObjectId { get; set; }
    public string ObjectType { get; set; }
    //public int? Priority { get; set; }
}