namespace ProjectBackend.Models.DTO;

public class PlaylistDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<MovieDto> Movies { get; set; } = new();
}

public class MovieDto
{
    public int Id { get; set; }
    public int TmdbId { get; set; }
    public string Title { get; set; }
}