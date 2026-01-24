using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectBackend.DB;
using ProjectBackend.Services.interfaces;

namespace ProjectBackend.Controllers;

[Authorize]
[ApiController]
[Route("image")]
public class ImageController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ImageController(ApplicationDbContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpGet("show-images")]
    public async Task<IActionResult> ShowImages()
    {
        var images = await _context.Movies
            .Select(m => new
            {
                PosterPath = m.PosterPath,
                BackdropPath = m.BackdropPath
            })
            .ToListAsync();

        return Ok(images);
    }

}
