using System.ComponentModel.DataAnnotations;

namespace ProjectBackend.Models.DTO;

public class RemoveRateIdDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public int movieId { get; set; }
}
