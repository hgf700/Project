using ProjectBackend.Models;

namespace ProjectBackend.Services.interfaces
{
    public interface ITmdbService
    {
        Task<TMDB_Response> GetMovieAsync(int movieId);
    }
}
