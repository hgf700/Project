using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectBackend.DB;
using ProjectBackend.Models.ReleatedToMovie;
using ProjectBackend.Models.ReleatedToSocial;
using ProjectBackend.Services.interfaces;
using System.Globalization;
using System.Security.Claims;

namespace ProjectBackend.Controllers;

[Authorize]
[ApiController]
[Route("movies")]
public class MoviesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ITmdbService _tmdbService;
    private readonly SeedGenresService _seedgenres;
    private readonly UserManager<ApplicationUser> _userManager;

    public MoviesController(
        ApplicationDbContext context,
        ITmdbService tmdbService,
        SeedGenresService seedgenres,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _tmdbService = tmdbService;
        _seedgenres = seedgenres;
        _userManager = userManager;
    }


    [Authorize]
    [HttpPost("add-from-tmdb")]
    public async Task<IActionResult> AddMoviesFromTmdb(int page = 1)
    {
        var tmdbMovies = await _tmdbService.GetPopularMoviesAsync(page);

        if (tmdbMovies == null || !tmdbMovies.Any())
            return NotFound("Brak filmów z TMDB");

        foreach (var tmdbMovie in tmdbMovies)
        {
            bool exists = await _context.Movies
                .AnyAsync(m => m.Title == tmdbMovie.OriginalTitle);

            if (exists)
                continue;

            DateTime releaseDate = DateTime.MinValue;

            if (!string.IsNullOrEmpty(tmdbMovie.ReleaseDate) &&
                DateTime.TryParseExact(
                    tmdbMovie.ReleaseDate,
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var parsedDate))
            {
                releaseDate = DateTime.SpecifyKind(parsedDate, DateTimeKind.Utc);
            }


            var movie = new Movie
            {
                TmdbId= tmdbMovie.TmdbId,
                Title = tmdbMovie.OriginalTitle,
                Overview = tmdbMovie.Overview,
                Adult = tmdbMovie.Adult,
                ReleaseDate = releaseDate,
                VoteAverage = (float)tmdbMovie.VoteAverage,
                PosterPath = tmdbMovie.PosterPath,
                BackdropPath = tmdbMovie.BackdropPath,
                MovieGenres = new List<MovieGenre>()
            };

            var genres = await _context.Genres
                .Where(g => tmdbMovie.GenreIds.Contains(g.TmdbId))
                .ToListAsync();

            foreach (var genre in genres)
            {
                movie.MovieGenres.Add(new MovieGenre
                {
                    Genre = genre
                });
            }

            _context.Movies.Add(movie);
        }

        await _context.SaveChangesAsync();

        return Ok("Filmy z TMDB zapisane do bazy");
    }

    [Authorize]
    [HttpPost("seed-genre")]
    public async Task<IActionResult> SeedGenreFromTmdb()
    {
        var TmdbGenres= await _seedgenres.GetAllGenresAsync();

        foreach (var genres in TmdbGenres)
        {
            bool exists = await _context.Genres
                .AnyAsync(m => m.TmdbId == genres.TmdbId);

            if (exists)
                continue;

            var genre = new Genre
            {
                TmdbId = genres.TmdbId,
                Name= genres.Name,
            };
            
            _context.Genres.Add(genre);
        }
        await _context.SaveChangesAsync();
        return Ok("Filmy z TMDB zapisane do bazy");
    }

    [Authorize]
    [HttpPost("rate")]
    public async Task<IActionResult> SendMovieRate(int num)
    {
        var TmdbGenres = await _seedgenres.GetAllGenresAsync();

        foreach (var genres in TmdbGenres)
        {
            bool exists = await _context.Genres
                .AnyAsync(m => m.TmdbId == genres.TmdbId);

            if (exists)
                continue;

            var genre = new Genre
            {
                TmdbId = genres.TmdbId,
                Name = genres.Name,
            };

            _context.Genres.Add(genre);
        }
        await _context.SaveChangesAsync();
        return Ok("Filmy z TMDB zapisane do bazy");
    }

    [Authorize]
    [HttpGet("show-movies")]
    public async Task<IActionResult> ShowMovies()
    {
        var UserEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        var currentUser = await _userManager.FindByEmailAsync(UserEmail);

        if (currentUser == null) 
            return Unauthorized();

        var movies = await _context.Movies.ToListAsync();

        return Ok(movies);
    }
}
