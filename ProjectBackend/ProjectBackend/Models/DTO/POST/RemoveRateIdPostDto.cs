using System.ComponentModel.DataAnnotations;

namespace ProjectBackend.Models.DTO.POST;

public class RemoveRateIdPostDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public int movieId { get; set; }
}
