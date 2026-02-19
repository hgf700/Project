namespace ProjectBackend.Models.ReleatedToMovie;

public class MovieActor
{
    public int MovieId { get; set; }
    public Movie Movie { get; set; } = null!;

    public int ActorId { get; set; }
    public Actor Actor { get; set; } = null!;

    public string Character { get; set; } = string.Empty;
    public int Order { get; set; }
}
