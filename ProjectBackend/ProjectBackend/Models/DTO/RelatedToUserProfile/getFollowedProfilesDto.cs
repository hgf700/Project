namespace ProjectBackend.Models.DTO.RelatedToUserProfile;

public class getFollowedProfilesDto
{
    public string UserId { get; set; }
    public string TargetUserId { get; set; }
    public string TargetUserEmail { get; set; }
}
