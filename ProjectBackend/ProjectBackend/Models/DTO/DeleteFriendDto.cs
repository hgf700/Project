using System.ComponentModel.DataAnnotations;

namespace ProjectBackend.Models.DTO;

public class DeleteFriendDto
{
    [Required]
    public string FriendId { get; set; }

}
