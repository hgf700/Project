using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectBackend.DB;
using ProjectBackend.Models;
using ProjectBackend.Services.interfaces;
using System.Globalization;

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
                Title = tmdbMovie.OriginalTitle,
                Overview = tmdbMovie.Overview,
                Adult = tmdbMovie.Adult,
                ReleaseDate = releaseDate,
                VoteAverage = (float)tmdbMovie.VoteAverage,
                PosterPath = tmdbMovie.PosterPath,
                BackdropPath = tmdbMovie.BackdropPath,
                MovieGenres = new List<MovieGenre>()
            };

            // mapowanie gatunków
            if (tmdbMovie.GenreIds != null)
            {
                foreach (var genreId in tmdbMovie.GenreIds)
                {
                    var genre = await _context.Genres
                        .FirstOrDefaultAsync(g => g.TmdbId == genreId);

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
        }

        await _context.SaveChangesAsync();

        return Ok("Filmy z TMDB zapisane do bazy");
    }

}
