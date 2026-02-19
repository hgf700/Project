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


    //public double CalculateScore(MovieUserPreference profile, Movie movie)
    //{
    //    double score = 0;

    //    foreach (var genre in movie.GenreIds)
    //    {
    //        if (profile.GenreWeights.TryGetValue(genre, out var weight))
    //            score += weight;
    //    }

    //    int bucket = (movie.ReleaseYear / 10) * 10;
    //    if (profile.YearBuckets.TryGetValue(bucket, out var yearWeight))
    //        score += yearWeight;

    //    return score;
    //}

}
