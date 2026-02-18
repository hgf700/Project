using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectBackend.DB;
using ProjectBackend.Models.DTO;
using ProjectBackend.Models.ReleatedToMovie;
using ProjectBackend.Models.ReleatedToPlaylist;
using ProjectBackend.Models.ReleatedToSocial;
using ProjectBackend.Services;
using ProjectBackend.Services.interfaces;
using System.Globalization;
using System.Security.Claims;

namespace ProjectBackend.Controllers.RelatedToMovies;

[Authorize]
[ApiController]
[Route("movies")]
public class MoviesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly SeedGenresService _seedgenres;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly TmdbImportService _importService;
    private readonly LoadActorsService _loadActorsService;


    public MoviesController(
        ApplicationDbContext context,
        SeedGenresService seedgenres,
        UserManager<ApplicationUser> userManager,
        TmdbImportService importService,
        LoadActorsService loadActorsService)
    {
        _context = context;
        _seedgenres = seedgenres;
        _userManager = userManager;
        _importService = importService;
        _loadActorsService = loadActorsService;
    }


    [HttpPost("import-from-tmdb")]
    public async Task<IActionResult> ImportFromTmdb(int page = 1)
    {
        await _importService.ImportMoviesWithGenresAsync(page);
        return Ok($"Zaimportowano filmy z gatunkami ze strony {page}");
    }

    [Authorize]
    [HttpGet("show-movies")]
    public async Task<IActionResult> ShowMovies()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var movies = await _context.Movies.ToListAsync();

        return Ok(movies);
    }

    [Authorize]
    [HttpGet("show-actors/{filmId}")]
    public async Task<IActionResult> ShowActors(int filmId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var actors = await _loadActorsService.GetTopActorsAsync(filmId);

        return Ok(actors);
    }

}
