using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectBackend.DB;
using ProjectBackend.Models.DTO.RelatedToMovies;
using ProjectBackend.Models.ReleatedToMovie;
using ProjectBackend.Models.ReleatedToSocial;
using System.Security.Claims;

namespace ProjectBackend.Controllers.RelatedToMovies;

[Authorize]
[ApiController]
[Route("preffered-genre")]
public class PrefferedGenreController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PrefferedGenreController(ApplicationDbContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpGet("show-genres")]
    public async Task<IActionResult> ShowGenres()
    {
        var genres = await _context.Genres
            .Select(g => new getGenresDto
            {
                GenreId = g.Id,
                GenreName = g.Name
            })
            .ToListAsync();

        return Ok(genres);
    }

    [Authorize]
    [HttpPost("choose-genre/{genreId}")]
    public async Task<IActionResult> ChooseGenre(int genreId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var exists = await _context.PrefferedGenres
            .AnyAsync(x => x.UserId == userId && x.GenreId == genreId);

        if (exists)
            return Ok(); // już istnieje – idempotentne

        var userGenre = new PrefferedGenre
        {
            GenreId = genreId,
            UserId = userId
        };

        _context.PrefferedGenres.Add(userGenre);
        await _context.SaveChangesAsync();

        return Ok();
    }


    [Authorize]
    [HttpDelete("remove-choosen-genre/{genreId}")]
    public async Task<IActionResult> RemoveChosenGenre(int genreId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var userGenre = await _context.PrefferedGenres
            .FirstOrDefaultAsync(x => x.UserId == userId && x.GenreId == genreId);

        if (userGenre == null)
            return NotFound();

        _context.PrefferedGenres.Remove(userGenre);
        await _context.SaveChangesAsync();

        return Ok();
    }

}
