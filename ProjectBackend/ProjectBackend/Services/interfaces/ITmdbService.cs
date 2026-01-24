using ProjectBackend.Models;

namespace ProjectBackend.Services.interfaces;
public interface ITmdbService
{
    Task<List<TMDB_Response>> GetPopularMoviesAsync(int page = 1);
}
