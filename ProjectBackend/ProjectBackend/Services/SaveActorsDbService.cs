//using ProjectBackend.DB;
//using ProjectBackend.Models.ReleatedToMovie;
//using ProjectBackend.Services.interfaces;

//namespace ProjectBackend.Services;

//public class SaveActorsDbService
//{
//    private readonly ApplicationDbContext _context;
//    private readonly LoadActorsService _loadActors;

//    public SaveActorsDbService(
//        ApplicationDbContext context,
//        LoadActorsService loadActors
//        )
//    {
//        _context = context;
//        _loadActors = loadActors;
//    }

//    public async Task SaveActorsDbAsync(int tmdbMovieId)
//    {
//        // 1️⃣ Pobierz film z bazy
//        var movie = await _context.Movies
//            .FirstOrDefaultAsync(m => m.TmdbId == tmdbMovieId);

//        if (movie == null)
//            throw new InvalidOperationException("Film nie istnieje w bazie.");

//        // 2️⃣ Pobierz aktorów z TMDB
//        var topActors = await _loadActors.GetTopActorsAsync(tmdbMovieId);

//        if (topActors == null || !topActors.Any())
//            throw new InvalidOperationException("Nie udało się pobrać listy aktorów z TMDB");

//        // 3️⃣ Pobierz już istniejących aktorów z bazy
//        var existingActors = await _context.Actors
//            .Where(a => topActors.Select(t => t.Id).Contains(a.TmdbId))
//            .ToListAsync();

//        var actorsMap = existingActors.ToDictionary(a => a.TmdbId, a => a);

//        foreach (var actorDto in topActors)
//        {
//            // 4️⃣ Jeśli aktor nie istnieje → dodaj
//            if (!actorsMap.TryGetValue(actorDto.Id, out var actor))
//            {
//                actor = new Actor
//                {
//                    TmdbId = actorDto.Id,
//                    Name = actorDto.Name,
//                    Popularity = actorDto.Popularity,
//                    ProfilePath = actorDto.ProfilePath
//                };

//                _context.Actors.Add(actor);
//                actorsMap[actorDto.Id] = actor;
//            }

//            // 5️⃣ Sprawdź czy relacja już istnieje
//            bool relationExists = await _context.MovieActors
//                .AnyAsync(ma => ma.MovieId == movie.Id && ma.ActorId == actor.Id);

//            if (!relationExists)
//            {
//                _context.MovieActors.Add(new MovieActor
//                {
//                    MovieId = movie.Id,
//                    ActorId = actor.Id,
//                    Order = actorDto.Order
//                });
//            }
//        }

//        await _context.SaveChangesAsync();
//    }


//}
