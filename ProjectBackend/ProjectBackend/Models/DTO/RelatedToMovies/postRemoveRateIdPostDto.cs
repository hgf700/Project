using System.ComponentModel.DataAnnotations;

namespace ProjectBackend.Models.DTO.RelatedToMovies;

public class postRemoveRateIdPostDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public int movieId { get; set; }
}
