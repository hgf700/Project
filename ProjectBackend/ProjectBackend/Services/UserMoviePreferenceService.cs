using ProjectBackend.Models.RelatedToRecommendation;
using ProjectBackend.Models.ReleatedToMovie;
using ProjectBackend.Models.ReleatedToSocial;

namespace ProjectBackend.Services;

public class UserMoviePreferenceService
{
    public void UpdateYearPreference(MovieUserPreference user, int year, RatingValue rate)
    {
        int bucket = (year / 10) * 10;

        if (!user.YearBuckets.ContainsKey(bucket))
            user.YearBuckets[bucket] = 0;

        user.YearBuckets[bucket] += (double)rate;
    }

    public void UpdateGenrePreference(MovieUserPreference user, int genreId, RatingValue rate)
    {
        if (!user.GenreWeights.ContainsKey(genreId))
            user.GenreWeights[genreId] = 0;

        user.GenreWeights[genreId] += (double)rate;
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
