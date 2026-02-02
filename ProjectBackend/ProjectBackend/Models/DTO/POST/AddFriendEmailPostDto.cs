using System.ComponentModel.DataAnnotations;

namespace ProjectBackend.Models.DTO.POST;

public class AddFriendEmailPostDto
{
    //[Required, EmailAddress]
    public string Email { get; init; }
}
