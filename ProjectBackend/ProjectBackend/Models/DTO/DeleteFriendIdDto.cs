using System.ComponentModel.DataAnnotations;

namespace ProjectBackend.Models.DTO;

public class DeleteFriendIdDto
{
    [Required]
    public string FriendId { get; set; }

}
