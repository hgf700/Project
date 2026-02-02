using System.ComponentModel.DataAnnotations;

namespace ProjectBackend.Models.DTO.POST;

public class DeleteFriendIdPostDto
{
    [Required]
    public string FriendId { get; set; }

}
