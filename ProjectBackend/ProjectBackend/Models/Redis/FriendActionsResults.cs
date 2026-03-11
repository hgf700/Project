using System.Text.Json.Serialization;

namespace ProjectBackend.Models.Redis;

public class FriendActionsResults
{
    [JsonPropertyName("total")]
    public long Total { get; set; }

    [JsonPropertyName("actions")]
    public required List<FriendAction> FriendActions { get; set; }
}

public class FriendAction
{
    [JsonPropertyName("idOfAction")]
    public required int IdOfAction { get; set; }

    [JsonPropertyName("friendId")]
    public int? FriendId { get; set; }

    [JsonPropertyName("friendNick")]
    public string? FriendNick { get; set; }

    [JsonPropertyName("friendCommittedAction")]
    public FriendActionType FriendCommittedAction { get; set; }     //Typ akcji wykonanej przez znajomego.likecommentsharefriend_request

    [JsonPropertyName("targetUserId")]
    public string TargetUserId { get; set; }

    [JsonPropertyName("objectId")]
    public int? ObjectId { get; set; }  //ID obiektu, którego dotyczy akcja. Przykłady: post id = 45

    [JsonPropertyName("objectType")]
    public string? ObjectType { get; set; } //Typ obiektu. postcommentphotoprofile

    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("updatedDate")]
    public DateTime? UpdatedDate { get; set; }

    [JsonPropertyName("seen")]
    public bool Seen { get; set; } = false;

    [JsonPropertyName("priority")]
    public int? Priority { get; set; } 
}

public enum FriendActionType
{
    Like,
    Comment,
    PostCreated
}