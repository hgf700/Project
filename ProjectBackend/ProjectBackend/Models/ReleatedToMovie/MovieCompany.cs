namespace ProjectBackend.Models.ReleatedToMovie;

public class MovieCompany
{
    public int MovieId { get; set; }
    public Movie Movie { get; set; } = null!;
    public int CompanyId { get; set; }
    public Company Company { get; set; } = null!;
}
