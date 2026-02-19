using ProjectBackend.Models.RelatedToRecommendation;
using ProjectBackend.Models.ReleatedToMovie;
using ProjectBackend.Models.ReleatedToSocial;
using System.Numerics;

namespace ProjectBackend.Services;

public class UserMoviePreferenceService
{
    public void UpdateYearPreference(MovieUserPreference user, int year, RatingValue rate)
    {
        if (user.YearBuckets == null)
            user.YearBuckets = new Dictionary<int, double>();

        int bucket = (year / 10) * 10;

        if (!user.YearBuckets.ContainsKey(bucket))
            user.YearBuckets[bucket] = 0;

        user.YearBuckets[bucket] += (double)rate;
    }

    public void UpdateGenrePreference(MovieUserPreference user, int genreId, RatingValue rate)
    {
        if (user.GenreWeights == null)
            user.GenreWeights = new Dictionary<int, double>();

        if (!user.GenreWeights.ContainsKey(genreId))
            user.GenreWeights[genreId] = 0;

        user.GenreWeights[genreId] += (double)rate;
    }

    public void TmdbRatingPreference(MovieUserPreference user, double tmdbRating, RatingValue rate)
    {
        if (user.TmdbRatingBuckets == null)
            user.TmdbRatingBuckets = new Dictionary<int, double>();

        int bucket = (int)Math.Floor(tmdbRating);

        if (!user.TmdbRatingBuckets.ContainsKey(bucket))
            user.TmdbRatingBuckets[bucket] = 0;

        user.TmdbRatingBuckets[bucket] += (double)rate;
    }

    public void ActorsPreference(MovieUserPreference user, int actorId, int order, RatingValue rate)
    {
        if (user.ActorWeights == null)
            user.ActorWeights = new Dictionary<int, double>();

        double roleWeight = 1.0 / (order + 1);

        if (!user.ActorWeights.ContainsKey(actorId))
            user.ActorWeights[actorId] = 0;

        user.ActorWeights[actorId] += (double)rate * roleWeight;
    }


    public double CalculateMovieScore(MovieUserPreference user, Movie movie)
    {
        double score = 0;

        // 1️⃣ Gatunki
        if (user.GenreWeights != null)
        {
            foreach (var genre in movie.MovieGenres)
            {
                if (user.GenreWeights.TryGetValue(genre.GenreId, out var value))
                    score += value * 2; // gatunki ważne
            }
        }

        // 2️⃣ Rok (dekada)
        if (user.YearBuckets != null)
        {
            int bucket = (movie.ReleaseDate.Year / 10) * 10;

            if (user.YearBuckets.TryGetValue(bucket, out var value))
                score += value * 1.2;
        }

        // 3️⃣ TMDB rating
        if (user.TmdbRatingBuckets != null)
        {
            int bucket = (int)Math.Floor(movie.VoteAverage);

            if (user.TmdbRatingBuckets.TryGetValue(bucket, out var value))
                score += value * 0.5; // mniejsza waga
        }

        // 4️⃣ Aktorzy
        //if (user.ActorWeights != null)
        //{
        //    foreach (var actor in movie.VoteAverage.Take(5))
        //    {
        //        if (user.ActorWeights.TryGetValue(actor.ActorId, out var value))
        //            score += value;
        //    }
        //}

        return score;
    }


}
