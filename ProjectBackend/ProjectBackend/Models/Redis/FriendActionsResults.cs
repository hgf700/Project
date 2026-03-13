using System.Text.Json.Serialization;

namespace ProjectBackend.Models.Redis;

public enum UserActionType
{
    Rate,
    Comment,
    PostCreated
}
public class UserActionsResults
{
    [JsonPropertyName("total")]
    public long Total { get; set; }

    [JsonPropertyName("actions")]
    public required List<UserAction> FriendActions { get; set; }
}

public class UserAction
{
    [JsonPropertyName("idOfAction")]
    public string IdOfAction { get; set; }

    [JsonPropertyName("userId")]
    public string? UserId { get; set; }

    [JsonPropertyName("userNick")]
    public string? UserNick { get; set; }

    [JsonPropertyName("userCommittedAction")] 
    public UserActionType FriendCommittedAction { get; set; } //Typ akcji wykonanej przez znajomego.likecommentsharefriend_request

    [JsonPropertyName("objectId")]
    public int? ObjectId { get; set; }  //ID obiektu, którego dotyczy akcja. Przykłady: post id = 45

    [JsonPropertyName("objectType")]
    public string? ObjectType { get; set; } //Typ obiektu. postcommentphotoprofile

    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("seen")]
    public bool Seen { get; set; } = false;

}

