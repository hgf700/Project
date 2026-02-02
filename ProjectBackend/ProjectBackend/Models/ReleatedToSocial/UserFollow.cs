namespace ProjectBackend.Models.ReleatedToSocial;

public class UserFollow
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; } // kto lajkuje
    public string TargetUserId { get; set; }
    public ApplicationUser TargetUser { get; set; } // kogo lajkuje
    public DateTime CreatedAt { get; set; }
}

