using ProjectBackend.DB;
using ProjectBackend.Models.ReleatedToMovie;
using Microsoft.EntityFrameworkCore;

namespace ProjectBackend.Services.Tmdb;
public class TmdbImportService
{
    private readonly ApplicationDbContext _context;
    private readonly TmdbService _tmdbService;
    private readonly TmdbSeedGenresService _genresService;
    private readonly TmdbSaveProductionCompaniesService _productionCompaniesService;

    

    public TmdbImportService(
        ApplicationDbContext context,
        TmdbService tmdbService,
        TmdbSeedGenresService genresService,
        TmdbSaveProductionCompaniesService productionCompaniesService)
    {
        _context = context;
        _tmdbService = tmdbService;
        _genresService = genresService;
        _productionCompaniesService = productionCompaniesService;
    }

    public async Task ImportMoviesWithGenresAsync(int page = 1)
    {
        var tmdbGenres = await _genresService.GetAllGenresAsync();

        if (tmdbGenres == null || !tmdbGenres.Any())
            throw new InvalidOperationException("Nie udało się pobrać listy gatunków z TMDB");

        var genreMap = await _context.Genres
            .ToDictionaryAsync(g => g.TmdbId);

        foreach (var tmdbGenre in tmdbGenres)
        {
            if (!genreMap.ContainsKey(tmdbGenre.TmdbId))
            {
                var newGenre = new Genre
                {
                    TmdbId = tmdbGenre.TmdbId,
                    Name = tmdbGenre.Name
                };

                _context.Genres.Add(newGenre);
                genreMap[tmdbGenre.TmdbId] = newGenre;
            }
        }

        await _context.SaveChangesAsync();

        var moviesDto = await _tmdbService.GetPopularMoviesAsync(page);
        if (moviesDto == null || !moviesDto.Any())
            return;

        var importedCount = 0;

        foreach (var movieDto in moviesDto)
        {
            var existingMovie = await _context.Movies
                .FirstOrDefaultAsync(m => m.TmdbId == movieDto.TmdbId);

            if (existingMovie != null)
                continue;

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
            await _context.SaveChangesAsync(); // potrzebne aby dostać newMovie.Id

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
                            MovieTitle = newMovie.Title,
                            GenreName = genre.Name,
                        });
                    }
                    else
                    {
                        Console.WriteLine($"Brak gatunku o TmdbId {tmdbGenreId} dla filmu {newMovie.Title}");
                    }
                }
            }

            var companiesDto = await _productionCompaniesService
                .GetProductionCompaniesAsync(movieDto.TmdbId); // <-- poprawione (TmdbId!)

            foreach (var companyDto in companiesDto)
            {
                // sprawdź czy firma już istnieje
                var existingCompany = await _context.Companies
                    .FirstOrDefaultAsync(c => c.CompanyTmdbId == companyDto.CompanyId);

                if (existingCompany == null)
                {
                    existingCompany = new Company
                    {
                        CompanyTmdbId = companyDto.CompanyId,
                        CompanyName = companyDto.CompanyName
                    };

                    _context.Companies.Add(existingCompany);
                    await _context.SaveChangesAsync();
                }

                _context.MovieCompanies.Add(new MovieCompany
                {
                    MovieId = newMovie.Id,
                    CompanyId = existingCompany.Id
                });
            }

            importedCount++;
        }

        await _context.SaveChangesAsync();

        Console.WriteLine(
            $"Zaimportowano {importedCount} nowych filmów z gatunkami i firmami (strona {page})");
    }
}

