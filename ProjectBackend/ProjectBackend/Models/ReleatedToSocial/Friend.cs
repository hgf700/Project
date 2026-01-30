namespace ProjectBackend.Models.ReleatedToSocial;

public class Friend
{
    public int Id { get; set; }

    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    public string FriendId { get; set; }
    public ApplicationUser FriendUser { get; set; }
}
