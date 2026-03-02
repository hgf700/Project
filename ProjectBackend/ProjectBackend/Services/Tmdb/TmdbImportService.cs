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
    private readonly TmdbLoadPeopleRoleService _loadPeopleRoleService;
    
    public TmdbImportService(
        ApplicationDbContext context,
        TmdbService tmdbService,
        TmdbSeedGenresService genresService,
        TmdbSaveProductionCompaniesService productionCompaniesService,
        TmdbLoadPeopleRoleService loadPeopleRoleService)
    {
        _context = context;
        _tmdbService = tmdbService;
        _genresService = genresService;
        _productionCompaniesService = productionCompaniesService;
        _loadPeopleRoleService = loadPeopleRoleService;
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
                            GenreId = genre.Id,
                            MovieTitle = newMovie.Title,
                            GenreName = genre.Name,
                        });
                    }
                }
            }

            var topActors = await _loadPeopleRoleService.GetTopPopularPeoplesAsync(movieDto.TmdbId);

            if (topActors == null || !topActors.Any())
            {
                Console.WriteLine("Brak aktorów");
            }
            else
            {
                var tmdbIds = topActors.Select(a => a.TmdbId).ToList();

                var existingActors = await _context.PeopleRoles
                    .Where(a => tmdbIds.Contains(a.TmdbId))
                    .ToListAsync();

                var actorsMap = existingActors.ToDictionary(a => a.TmdbId);

                foreach (var actorDto in topActors)
                {
                    if (!actorsMap.TryGetValue(actorDto.TmdbId, out var actorEntity))
                    {
                        actorEntity = new PeopleRole
                        {
                            TmdbId = actorDto.TmdbId,
                            OriginalName = actorDto.OriginalName,
                            Popularity = actorDto.Popularity,
                            ProfilePath = actorDto.ProfilePath,
                            KnownFor=actorDto.KnownFor,
                        };

                        _context.PeopleRoles.Add(actorEntity);
                        await _context.SaveChangesAsync(); // potrzebne żeby mieć Id

                        actorsMap[actorDto.TmdbId] = actorEntity;
                    }

                    var exists = await _context.MoviePeopleRoles
                        .AnyAsync(mpr =>
                            mpr.MovieId == newMovie.Id &&
                            mpr.PeopleRolesId == actorEntity.Id);

                    if (!exists)
                    {
                        _context.MoviePeopleRoles.Add(new MoviePeopleRole
                        {
                            MovieId = newMovie.Id,
                            PeopleRolesId = actorEntity.Id,
                            Character=actorDto.Character,
                            Order=actorDto.Order,
                            Department=actorDto.Department,
                            Job = actorDto.Job
                        });
                    }

                }
            }
            await _context.SaveChangesAsync();

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

            var tagList = new List<string>();

            // Gatunki
            tagList.AddRange(
                genreMap
                    .Where(g => movieDto.GenreIds.Contains(g.Key))
                    .Select(g => g.Value.Name.Replace(" ", "").ToLower())
            );

            var people = await _context.MoviePeopleRoles
                .Where(r => r.MovieId == newMovie.Id)
                .Select(r => new { r.Job, r.PeopleRole.OriginalName })
                .ToListAsync();

            tagList.AddRange(
                people.Where(p => p.Job == "Director")
                      .Select(p => p.OriginalName.Replace(" ", "").ToLower())
            );

            tagList.AddRange(
                people.Where(p => p.Job == "Actor")
                      .Select(p => p.OriginalName.Replace(" ", "").ToLower())
            );

            // Firmy
            tagList.AddRange(companiesDto
                .Select(c => c.CompanyName.Replace(" ", "").ToLower()));

            tagList.Add(newMovie.Overview.ToLower());

            // Rok
            tagList.Add(newMovie.ReleaseDate.Year.ToString());

            var finalTags = string.Join(" ",
                tagList
                    .Where(t => !string.IsNullOrWhiteSpace(t))
                    .Distinct()
            );

            if (!existingTags.TryGetValue(finalTags, out var tagEntity))
            {
                tagEntity = new RecomendTag { Tag = finalTags };
                _context.RecomendTags.Add(tagEntity);
                existingTags[finalTags] = tagEntity;

                await _context.SaveChangesAsync();
            }

            _context.RecomendTagMovies.Add(new RecomendTagMovie
            {
                MovieId= newMovie.Id,
                RecomendTagId = tagEntity.Id,
            });

            importedCount++;

            await _context.SaveChangesAsync();
        }

        Console.WriteLine($"Zaimportowano {importedCount} nowych filmów (strona {page})");
    }
}

