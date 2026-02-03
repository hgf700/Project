using System.ComponentModel.DataAnnotations;

namespace ProjectBackend.Models.DTO.RelatedToUserProfile;

public class postAddFriendEmailPostDto
{
    //[Required, EmailAddress]
    public string Email { get; init; }
}
