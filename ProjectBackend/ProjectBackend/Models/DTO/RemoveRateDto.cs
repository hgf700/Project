using System.ComponentModel.DataAnnotations;

namespace ProjectBackend.Models.DTO;

public class RemoveRateDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public int movieId { get; set; }
}
