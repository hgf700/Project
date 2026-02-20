using ProjectBackend.DB;
using ProjectBackend.Models.ReleatedToMovie;
using Microsoft.EntityFrameworkCore;

namespace ProjectBackend.Services;
public class TmdbImportService
{
    private readonly ApplicationDbContext _context;
    private readonly TmdbService _tmdbService;
    private readonly SeedGenresService _genresService;

    public TmdbImportService(
        ApplicationDbContext context,
        TmdbService tmdbService,
        SeedGenresService genresService)
    {
        _context = context;
        _tmdbService = tmdbService;
        _genresService = genresService;
    }

    public async Task ImportMoviesWithGenresAsync(int page = 1)
    {
        // Krok 1: Pobierz i zapisz gatunki (tylko jeśli ich nie ma)
        var tmdbGenres = await _genresService.GetAllGenresAsync();

        if (tmdbGenres == null || !tmdbGenres.Any())
        {
            throw new InvalidOperationException("Nie udało się pobrać listy gatunków z TMDB");
        }

        // Mapa TmdbId → Genre (żeby szybciej znajdować)
        var genreMap = new Dictionary<int, Genre>();

        foreach (var tmdbGenre in tmdbGenres)
        {
            var existing = await _context.Genres
                .FirstOrDefaultAsync(g => g.TmdbId == tmdbGenre.TmdbId);

            if (existing == null)
            {
                var newGenre = new Genre
                {
                    TmdbId = tmdbGenre.TmdbId,
                    Name = tmdbGenre.Name
                };
                _context.Genres.Add(newGenre);
                genreMap[tmdbGenre.TmdbId] = newGenre;
            }
            else
            {
                genreMap[tmdbGenre.TmdbId] = existing;
            }
        }

        await _context.SaveChangesAsync(); // Zapisz nowe gatunki

        // Krok 2: Pobierz filmy
        var moviesDto = await _tmdbService.GetPopularMoviesAsync(page);

        if (moviesDto == null || !moviesDto.Any())
        {
            return; // Nic do zrobienia
        }

        var importedCount = 0;

        foreach (var movieDto in moviesDto)
        {
            // Sprawdź czy film już istnieje
            var existingMovie = await _context.Movies
                .FirstOrDefaultAsync(m => m.TmdbId == movieDto.TmdbId);

            if (existingMovie != null) continue; // Pomijamy duplikaty

            DateTime releaseDate = DateTime.MinValue;
            if (DateTime.TryParse(movieDto.ReleaseDate, out var d))
                releaseDate = DateTime.SpecifyKind(d, DateTimeKind.Utc);

            var newMovie = new Movie
            {
                TmdbId = movieDto.TmdbId,
                Title = movieDto.Title ?? movieDto.OriginalTitle,
                Overview = movieDto.Overview,
                ReleaseDate = releaseDate,
                VoteAverage = (float)movieDto.VoteAverage,
                PosterPath = movieDto.PosterPath,
                BackdropPath = movieDto.BackdropPath
            };

            _context.Movies.Add(newMovie);
            await _context.SaveChangesAsync(); // Zapisz film, żeby dostać ID

            // Krok 3: Powiąż gatunki
            if (movieDto.GenreIds != null && movieDto.GenreIds.Length > 0)
            {
                foreach (var tmdbGenreId in movieDto.GenreIds)
                {
                    if (genreMap.TryGetValue(tmdbGenreId, out var genre))
                    {
                        _context.MovieGenres.Add(new MovieGenre
                        {
                            MovieId = newMovie.Id,
                            GenreId = genre.Id,
                            MovieTitle=newMovie.Title,
                            GenreName=genre.Name,
                        });
                    }
                    else
                    {
                        // Opcjonalnie loguj brak gatunku
                        Console.WriteLine($"Brak gatunku o TmdbId {tmdbGenreId} dla filmu {newMovie.Title}");
                    }
                }
            }

            importedCount++;
        }

        await _context.SaveChangesAsync();

        Console.WriteLine($"Zaimportowano {importedCount} nowych filmów z powiązaniami gatunków (strona {page})");
    }
}
