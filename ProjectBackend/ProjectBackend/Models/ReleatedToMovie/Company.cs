namespace ProjectBackend.Models.ReleatedToMovie;

public class Company
{
    public int Id { get; set; }
    public int CompanyTmdbId { get; set; }
    public string CompanyName { get; set; }
    public ICollection<MovieCompany> MovieCompanies { get; set; } = new List<MovieCompany>();

}
