using System.Text.Json.Serialization;

namespace ProjectBackend.Models.ReleatedToMovie;

public enum KnownForDepartment
{
    Acting = 0,
    Directing = 1,
    Writing = 2,
    Production = 3,
    Camera = 4
}

public class PeopleRole
{
    public int Id { get; set; }
    public int TmdbId { get; set; } 
    public string OriginalName { get; set; } = null;
    public string ProfilePath { get; set; } = null;
    public KnownForDepartment KnownFor { get; set; }
    public double Popularity { get; set; }
    public string Job { get; set; }
    public ICollection<MoviePeopleRole> MoviePeopleRoles { get; set; } = new List<MoviePeopleRole>();

}
