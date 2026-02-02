namespace ProjectBackend.Models.DTO.GET;

public class FollowedProfilesDto
{
    public string UserId { get; set; }
    public string TargetUserId { get; set; }
    public string TargetUserEmail { get; set; }
}
