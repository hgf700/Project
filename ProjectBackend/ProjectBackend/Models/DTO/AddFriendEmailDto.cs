using System.ComponentModel.DataAnnotations;

namespace ProjectBackend.Models.DTO;

public class AddFriendEmailDto
{
    [Required, EmailAddress]
    public string Email { get; init; }
}
