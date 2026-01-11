namespace ProjectBackend.Models;

public class Friend
{
    public int Id { get; set; }

    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

    public string FriendUserId { get; set; }
    public ApplicationUser FriendUser { get; set; }
}
