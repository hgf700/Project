using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectBackend.DB;
using ProjectBackend.Models.DTO;
using ProjectBackend.Models.ReleatedToMovie;
using ProjectBackend.Models.ReleatedToPlaylist;
using ProjectBackend.Models.ReleatedToSocial;
using ProjectBackend.Services.interfaces;
using ProjectBackend.Services.Redis;
using ProjectBackend.Services.Tmdb;
using System.Globalization;
using System.Security.Claims;

namespace ProjectBackend.Controllers.RelatedToMovies;

[Authorize]
[ApiController]
[Route("movies")]
public class MoviesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly TmdbSeedGenresService _seedgenres;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly TmdbImportService _importService;
    private readonly TmdbLoadPeopleRoleService _loadActorsService;
    public MoviesController(
        ApplicationDbContext context,
        TmdbSeedGenresService seedgenres,
        UserManager<ApplicationUser> userManager,
        TmdbImportService importService,
        TmdbLoadPeopleRoleService loadActorsService
    
        )
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
    [HttpGet("show-selected-movie/{tmdbId}")]
    public async Task<IActionResult> ShowSelectedMovie(int tmdbId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var movie = await _context.Movies.FirstOrDefaultAsync(sm => sm.TmdbId == tmdbId);

        return Ok(movie);
    }

    [Authorize]
    [HttpGet("show-actors/{filmId}")]
    public async Task<IActionResult> ShowActors(int filmId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var actors = await _loadActorsService.GetTopPopularPeoplesAsync(filmId);

        return Ok(actors);
    }

}
