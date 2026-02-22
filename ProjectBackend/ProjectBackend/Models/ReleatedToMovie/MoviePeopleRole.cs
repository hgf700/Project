namespace ProjectBackend.Models.ReleatedToMovie;

public class MoviePeopleRole
{
    public int MovieId { get; set; }
    public Movie Movie { get; set; } = null!;

    public int PeopleRolesId { get; set; }
    public PeopleRole PeopleRoles { get; set; } = null!;

    public string Character { get; set; } = string.Empty;
    public int Order { get; set; }
    public string Job { get; set; }
}
