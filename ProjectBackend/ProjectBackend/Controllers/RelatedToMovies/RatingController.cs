using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectBackend.DB;
using ProjectBackend.Models.DTO;
using ProjectBackend.Models.DTO.RelatedToMovies;
using ProjectBackend.Models.RelatedToRecommendation;
using ProjectBackend.Models.ReleatedToPlaylist;
using ProjectBackend.Models.ReleatedToSocial;
using ProjectBackend.Services;
using System.ComponentModel.Design;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjectBackend.Controllers.RelatedToMovies;

[Authorize]
[ApiController]
[Route("rating")]
public class RatingController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public RatingController(
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext context
        )
    {
        _userManager = userManager;
        _context = context;
    }

    [Authorize]
    [HttpGet("show-user-rates")]
    public async Task<IActionResult> ShowPlaylistValues()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var rates = await _context.PlaylistLikes
            .Where(p =>p.UserId == userId)
            .Select(p => new getUserRatedMedia
            {
                PlaylistId= p.PlaylistId,
                UserId=p.UserId,
                CreatedAt= p.CreatedAt,
            })
            .FirstOrDefaultAsync();

        if (rates == null)
            return NotFound("Playlist not found");

        return Ok(rates);
    }
    
    [Authorize]
    [HttpPost("rate-movie")]
    public async Task<IActionResult> RateMovie(int movieId, [FromBody] postRateMoviePostDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var movie = await _context.Movies
            .Include(m => m.MovieGenre) // WAŻNE
            .FirstOrDefaultAsync(m => m.TmdbId == movieId);

        if (movie == null)
            return NotFound("Movie not found");

        var entry = await _context.UserMediaStatuses
            .FirstOrDefaultAsync(x => x.UserId == userId && x.MovieId == movie.Id);

        var userPreference = await _context.MovieUserPreferences
            .FirstOrDefaultAsync(x => x.UserId == userId);

        if (userPreference == null)
        {
            userPreference = new MovieUserPreference
            {
                UserId = userId
            };

            _context.MovieUserPreferences.Add(userPreference);
        }

        if (entry == null)
        {
            entry = new UserMediaStatus
            {
                UserId = userId,
                MovieId = movie.Id,
                Rating = dto.Rating
            };

            _context.UserMediaStatuses.Add(entry);

            var releaseYear = movie.ReleaseDate.Year;

            //_moviePreferenceService.UpdateYearPreference(
            //    userPreference,
            //    releaseYear,
            //    dto.Rating
            //);

            //foreach (var genre in movie.MovieGenres)
            //{
            //    _moviePreferenceService.UpdateGenrePreference(
            //        userPreference,
            //        genre.GenreId,
            //        dto.Rating
            //    );
            //}

            //_moviePreferenceService.TmdbRatingPreference(
            //    userPreference,
            //    movie.VoteAverage,
            //    dto.Rating
            //);

            //foreach (var movieActor in movie.MoviePeopleRoles.Take(5))
            //{
            //    _moviePreferenceService.ActorsPreference(
            //        userPreference,
            //        movieActor.PeopleRoles.TmdbId,
            //        movieActor.Order,
            //        dto.Rating
            //    );
            //}


        }
        else
        {
            var oldRating = entry.Rating;

        }
        //else
        //{
        //    var oldRating = entry.Rating;

        //    // odejmij wpływ starej oceny
        //    _moviePreferenceService.RemoveYearPreference(userPreference, movie.ReleaseDate.Year, oldRating);

        //    foreach (var genre in movie.MovieGenres)
        //    {
        //        _moviePreferenceService.RemoveGenrePreference(userPreference, genre.GenreId, oldRating);
        //    }

        //    _moviePreferenceService.RemoveTmdbRatingPreference(userPreference, movie.VoteAverage, oldRating);

        //    // ustaw nową ocenę
        //    entry.Rating = dto.Rating;

        //    // dodaj nową wagę
        //    _moviePreferenceService.UpdateYearPreference(userPreference, movie.ReleaseDate.Year, dto.Rating);

        //    foreach (var genre in movie.MovieGenres)
        //    {
        //        _moviePreferenceService.UpdateGenrePreference(userPreference, genre.GenreId, dto.Rating);
        //    }

        //    _moviePreferenceService.TmdbRatingPreference(userPreference, movie.VoteAverage, dto.Rating);
        //}



        await _context.SaveChangesAsync();
        return Ok();
    }

    [Authorize]
    [HttpPost("remove-rate")]
    public async Task<IActionResult> RemoveRate([FromBody] postRemoveRateIdPostDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var entry = await _context.UserMediaStatuses
            .FirstOrDefaultAsync(x => x.UserId == userId && x.MovieId == dto.movieId);

        if (entry == null)  
            return NotFound();

        _context.UserMediaStatuses.Remove(entry);

        await _context.SaveChangesAsync();
        return Ok();
    }


}
