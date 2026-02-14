using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Tasks;
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

    public MoviesController(
        ApplicationDbContext context,
        SeedGenresService seedgenres,
        UserManager<ApplicationUser> userManager,
        TmdbImportService importService)
    {
        _context = context;
        _seedgenres = seedgenres;
        _userManager = userManager;
        _importService = importService;
    }


    [HttpPost("import-from-tmdb")]
    public async Task<IActionResult> ImportFromTmdb(int page = 1)
    {
        await _importService.ImportMoviesWithGenresAsync(page);
        return Ok($"Zaimportowano filmy z gatunkami ze strony {page}");
    }

    //[Authorize]
    //[HttpPost("add-from-tmdb")]
    //public async Task<IActionResult> AddMoviesFromTmdb(int page = 1)
    //{
    //    _context.ChangeTracker.Clear();

    //    var tmdbGenres = await _seedgenres.GetAllGenresAsync();
    //    var existingGenreIds = await _context.Genres.Select(g => g.TmdbId).ToListAsync();

    //    foreach (var gDto in tmdbGenres)
    //    {
    //        if (!existingGenreIds.Contains(gDto.TmdbId))
    //        {
    //            _context.Genres.Add(new Genre { TmdbId = gDto.TmdbId, Name = gDto.Name });
    //        }
    //    }

    //    // ZAPISUJEMY GATUNKI TERAZ - żeby baza nadała im ID
    //    await _context.SaveChangesAsync();

    //    // 2. TERAZ pobieramy filmy
    //    var tmdbMovies = await _tmdbService.GetPopularMoviesAsync(page);

    //    // Pobieramy świeżą mapę ID (już z nowymi gatunkami)
    //    var genreMap = await _context.Genres.ToDictionaryAsync(x => x.TmdbId, x => x.Id);

    //    var newMovies = new List<Movie>();
    //    foreach (var tmdbMovie in tmdbMovies)
    //    {
    //        if (await _context.Movies.AnyAsync(m => m.TmdbId == tmdbMovie.TmdbId)) continue;

    //        DateTime releaseDate = DateTime.MinValue;
    //        if (DateTime.TryParse(tmdbMovie.ReleaseDate, out var d))
    //            releaseDate = DateTime.SpecifyKind(d, DateTimeKind.Utc);

    //        var movie = new Movie
    //        {
    //            TmdbId = tmdbMovie.TmdbId,
    //            Title = tmdbMovie.Title, // Używaj Title dla spójności
    //            Overview = tmdbMovie.Overview,
    //            Adult = tmdbMovie.Adult,
    //            ReleaseDate = releaseDate,
    //            VoteAverage = (float)tmdbMovie.VoteAverage,
    //            PosterPath = tmdbMovie.PosterPath,
    //            BackdropPath = tmdbMovie.BackdropPath,
    //            MovieGenres = new List<MovieGenre>()
    //        };

    //        foreach (var gid in tmdbMovie.GenreIds)
    //        {
    //            if (genreMap.TryGetValue(gid, out int localId))
    //            {
    //                movie.MovieGenres.Add(new MovieGenre { GenreId = localId });
    //            }
    //        }
    //        newMovies.Add(movie);
    //    }

    //    _context.Movies.AddRange(newMovies);
    //    await _context.SaveChangesAsync();

    //    return Ok("Zsynchronizowano gatunki i filmy w jednej sesji.");

    //}

    //[Authorize]
    //[HttpPost("seed-genre")]
    //public async Task<IActionResult> SeedGenreFromTmdb()
    //{
    //    var TmdbGenres= await _seedgenres.GetAllGenresAsync();

    //    foreach (var genres in TmdbGenres)
    //    {
    //        bool exists = await _context.Genres
    //            .AnyAsync(m => m.TmdbId == genres.TmdbId);

    //        if (exists)
    //            continue;

    //        var genre = new Genre
    //        {
    //            TmdbId = genres.TmdbId,
    //            Name= genres.Name,
    //        };

    //        _context.Genres.Add(genre);
    //    }
    //    await _context.SaveChangesAsync();
    //    return Ok("Filmy z TMDB zapisane do bazy");
    //}

    [Authorize]
    [HttpGet("show-movies")]
    public async Task<IActionResult> ShowMovies()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var movies = await _context.Movies.ToListAsync();

        return Ok(movies);
    }


}
