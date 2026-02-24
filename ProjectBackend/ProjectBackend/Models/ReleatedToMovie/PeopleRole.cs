using System.Text.Json.Serialization;

namespace ProjectBackend.Models.ReleatedToMovie;

public class PeopleRole
{
    public int Id { get; set; }
    public int TmdbId { get; set; }
    public string OriginalName { get; set; } = null;
    public string ?ProfilePath { get; set; } 
    public double ?Popularity { get; set; }
    public string ?KnownFor { get; set; }
    public ICollection<MoviePeopleRole> MoviePeopleRole { get; set; } = new List<MoviePeopleRole>();
}
