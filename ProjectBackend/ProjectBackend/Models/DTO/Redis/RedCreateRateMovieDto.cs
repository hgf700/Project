using ProjectBackend.Models.Redis;

namespace ProjectBackend.Models.DTO.Redis;

public class RedCreateRateMovieDto
{
    public string userId { get; set; }
    public string userNick { get; set; }
    public int movieId { get; set; }
    public UserActionType FriendCommittedAction { get; set; }
}
