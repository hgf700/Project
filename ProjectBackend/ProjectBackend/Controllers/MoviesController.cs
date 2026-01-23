using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectBackend.DB;
using ProjectBackend.Models;
using ProjectBackend.Services.interfaces;

namespace ProjectBackend.Controllers;

[Authorize]
[ApiController]
[Route("movies")]
public class MoviesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ITmdbService _tmdbService;

    public MoviesController(ApplicationDbContext context, ITmdbService tmdbService)
    {
        _context = context;
        _tmdbService = tmdbService;
    }

    [HttpPost("add-from-tmdb/{id}")]
    public async Task<IActionResult> AddMovieFromTmdb(int id)
    {
        var tmdbMovie = await _tmdbService.GetMovieAsync(id);

        if (tmdbMovie == null) return NotFound();

        // Parsowanie release_date
        DateTime? releaseDate = null;
        if (!string.IsNullOrEmpty(tmdbMovie.ReleaseDate))
        {
            if (DateTime.TryParse(tmdbMovie.ReleaseDate, out var parsedDate))
                releaseDate = parsedDate;
        }

        // Mapowanie TMDB_Response na Movie w bazie
        var movie = new Movie
        {
            Title = tmdbMovie.OriginalTitle,
            Overview = tmdbMovie.Overview,
            Adult = tmdbMovie.Adult,
            GenreIds = tmdbMovie.GenreIds,
            ReleaseDate = releaseDate ?? DateTime.MinValue,
            VoteAverage = (float)tmdbMovie.VoteAverage,
            PosterPath = tmdbMovie.PosterPath,
            BackdropPath = tmdbMovie.BackdropPath
        };

        // Mapowanie gatunków na relacje wiele-do-wielu
        if (tmdbMovie.GenreIds != null)
        {
            foreach (var genreId in tmdbMovie.GenreIds)
            {
                var genre = await _context.Genres.FirstOrDefaultAsync(g => g.TmdbId == genreId);
                if (genre != null)
                {
                    movie.MovieGenres.Add(new MovieGenre
                    {
                        Movie = movie,
                        Genre = genre
                    });
                }
            }
        }

        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();

        return Ok(movie);
    }
}
