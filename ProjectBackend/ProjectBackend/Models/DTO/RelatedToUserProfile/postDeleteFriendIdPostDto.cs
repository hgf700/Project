using System.ComponentModel.DataAnnotations;

namespace ProjectBackend.Models.DTO.RelatedToUserProfile;

public class postDeleteFriendIdPostDto
{
    [Required]
    public string FriendId { get; set; }

}
