namespace ProjectBackend.Models.DTO;

public class ProfileMessageDto
{
    public string AuthorId { get; set; }
    public string AuthorEmail { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Text { get; set; }
}
