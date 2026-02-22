using Microsoft.EntityFrameworkCore;
using ProjectBackend.DB;
using ProjectBackend.Models.RelatedToRecommendation;
using ProjectBackend.Models.ReleatedToMovie;

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

        var existingTags = await _context.RecomendTags
            .ToDictionaryAsync(t => t.Tag, t => t);

        var moviesDto = await _tmdbService.GetPopularMoviesAsync(page);
        if (moviesDto == null || !moviesDto.Any())
            return;

        var importedCount = 0;

        foreach (var movieDto in moviesDto)
        {
            if (await _context.Movies.AnyAsync(m => m.TmdbId == movieDto.TmdbId))
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
            await _context.SaveChangesAsync(); // tylko raz — potrzebne do Id

            // 🎬 GENRES
            if (movieDto.GenreIds != null)
            {
                foreach (var tmdbGenreId in movieDto.GenreIds)
                {
                    if (genreMap.TryGetValue(tmdbGenreId, out var genre))
                    {
                        _context.MovieGenres.Add(new MovieGenre
                        {
                            MovieId = newMovie.Id,
                            GenreId = genre.Id
                        });
                    }
                }
            }

            // 🏢 COMPANIES
            var companiesDto = await _productionCompaniesService
                .GetProductionCompaniesAsync(movieDto.TmdbId);

            foreach (var companyDto in companiesDto)
            {
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
                    await _context.SaveChangesAsync(); // tylko gdy nowa firma
                }

                _context.MovieCompanies.Add(new MovieCompany
                {
                    MovieId = newMovie.Id,
                    CompanyId = existingCompany.Id
                });
            }

            await _context.SaveChangesAsync();

            // 🔥 BUDOWANIE TAGÓW — BEZ Include

            var tagList = new List<string>();

            // Gatunki
            tagList.AddRange(
                genreMap
                    .Where(g => movieDto.GenreIds.Contains(g.Key))
                    .Select(g => g.Value.Name.Replace(" ", ""))
            );

            // Firmy
            tagList.AddRange(companiesDto
                .Select(c => c.CompanyName.Replace(" ", "")));

            // Rok
            tagList.Add(newMovie.ReleaseDate.Year.ToString());

            // PEOPLE (jeśli masz już zapisane wcześniej)
            var people = await _context.MoviePeopleRoles
                .Where(r => r.MovieId == newMovie.Id)
                .Select(r => new { r.Job, r.PeopleRoles.OriginalName })
                .ToListAsync();

            tagList.AddRange(people.Select(p => p.Job.Replace(" ", "")));

            tagList.AddRange(
                people.Where(p => p.Job == "Director")
                      .Select(p => p.OriginalName.Replace(" ", ""))
            );

            tagList.AddRange(
                people.Where(p => p.Job == "Actor")
                      .Take(3)
                      .Select(p => p.OriginalName.Replace(" ", ""))
            );

            tagList = tagList
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Distinct()
                .ToList();

            foreach (var tag in tagList)
            {
                if (!existingTags.TryGetValue(tag, out var tagEntity))
                {
                    tagEntity = new RecomendTag { Tag = tag };
                    _context.RecomendTags.Add(tagEntity);
                    existingTags[tag] = tagEntity;
                }

                _context.RecomendTagMovies.Add(new RecomendTagMovie
                {
                    MovieId = newMovie.Id,
                    RecomendTag = tagEntity
                });
            }

            importedCount++;
        }

        await _context.SaveChangesAsync();

        Console.WriteLine($"Zaimportowano {importedCount} nowych filmów (strona {page})");
    }
}

