using System.Text.Json.Serialization;

namespace ProjectBackend.Models.ReleatedToMovie;

public class Actor
{
    public int Id { get; set; }
    public int TmdbId { get; set; }
    public string OriginalName { get; set; }
    public string ProfilePath { get; set; }
    public string KnownFor { get; set; }
    public double Popularity { get; set; }
    public ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>();

}
