using System.ComponentModel.DataAnnotations;

namespace ProjectBackend.DTO;

public class AddFriendDto
{
    [Required, EmailAddress]
    public string Email { get; init; }
}
