//using Microsoft.EntityFrameworkCore;
//using ProjectBackend.DB;
//using ProjectBackend.Models.RelatedToRecommendation;
//using ProjectBackend.Models.ReleatedToMovie;
//using ProjectBackend.Models.ReleatedToSocial;
//using System.Numerics;

//namespace ProjectBackend.Services;

//public class UserMoviePreferenceService
//{
//    //private readonly ApplicationDbContext _context;
//    //UserMoviePreferenceService(ApplicationDbContext context
//    //    )
//    //{
//    //    _context= context;
//    //}
//    public void UpdateYearPreference(MovieUserPreference user, int year, RatingValue rate)
//    {
//        if (user.YearBuckets == null)
//            user.YearBuckets = new Dictionary<int, double>();

//        int bucket = (year / 10) * 10;

//        if (!user.YearBuckets.ContainsKey(bucket))
//            user.YearBuckets[bucket] = 0;

//        user.YearBuckets[bucket] += (double)rate;
//    }

//    public void UpdateGenrePreference(MovieUserPreference user, int genreId, RatingValue rate)
//    {
//        if (user.GenreWeights == null)
//            user.GenreWeights = new Dictionary<int, double>();

//        if (!user.GenreWeights.ContainsKey(genreId))
//            user.GenreWeights[genreId] = 0;

//        user.GenreWeights[genreId] += (double)rate;
//    }

//    public void TmdbRatingPreference(MovieUserPreference user, double tmdbRating, RatingValue rate)
//    {
//        if (user.TmdbRatingBuckets == null)
//            user.TmdbRatingBuckets = new Dictionary<int, double>();

//        int bucket = (int)Math.Floor(tmdbRating);

//        if (!user.TmdbRatingBuckets.ContainsKey(bucket))
//            user.TmdbRatingBuckets[bucket] = 0;

//        user.TmdbRatingBuckets[bucket] += (double)rate;
//    }

//    public void ActorsPreference(MovieUserPreference user, int actorId, int order, RatingValue rate)
//    {
//        if (user.ActorWeights == null)
//            user.ActorWeights = new Dictionary<int, double>();

//        double roleWeight = 1.0 / (order + 1);

//        if (!user.ActorWeights.ContainsKey(actorId))
//            user.ActorWeights[actorId] = 0;

//        user.ActorWeights[actorId] += (double)rate * roleWeight;
//    }


//    public async Task<double> CalculateMovieScore(MovieUserPreference user, Movie movie)
//    {
//        double score = 0;

//        if (user.GenreWeights != null)
//        {
//            foreach (var genre in movie.MovieGenres)
//            {
//                if (user.GenreWeights.TryGetValue(genre.GenreId, out var value))
//                    score += value * 2; 
//            }
//        }

//        if (user.YearBuckets != null)
//        {
//            int bucket = (movie.ReleaseDate.Year / 10) * 10;

//            if (user.YearBuckets.TryGetValue(bucket, out var value))
//                score += value * 1.2;
//        }

//        if (user.TmdbRatingBuckets != null)
//        {
//            int bucket = (int)Math.Floor(movie.VoteAverage);

//            if (user.TmdbRatingBuckets.TryGetValue(bucket, out var value))
//                score += value * 0.5; 
//        }

//        if (user.ActorWeights != null)
//        {
//            foreach (var movieActor in movie.MoviePeopleRole)
//            {
//                var actor = movieActor.PeopleRoles;

//                if (user.ActorWeights.TryGetValue(actor.TmdbId, out var value))
//                {
//                    double roleWeight = 1.0 / (movieActor.Order + 1);
//                    score += value * roleWeight * actor.Popularity;
//                }
//            }
//        }

//        return score;
//    }
//}
