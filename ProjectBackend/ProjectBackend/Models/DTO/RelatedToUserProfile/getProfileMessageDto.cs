namespace ProjectBackend.Models.DTO.RelatedToUserProfile;

public class getProfileMessageDto
{
    public int Id { get; set; }
    public string AuthorId { get; set; }
    public string AuthorEmail { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Text { get; set; }
}
