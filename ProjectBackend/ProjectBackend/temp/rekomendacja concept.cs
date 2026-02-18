

//public static class EventWeights
//{
//    public static readonly Dictionary<string, double> Weights = new()
//    {
//        { "play", 3 },
//        { "details", 1 },
//        { "cast", 2 },
//        { "ratingClick", 2 },
//        { "quickExit", -1 }
//    };
//}

//public void UpdateUserProfile(UserProfile profile, MovieFeatures movie, string eventType)
//{
//    if (!EventWeights.Weights.ContainsKey(eventType))
//        return;

//    var weight = EventWeights.Weights[eventType];

//    foreach (var genre in movie.Genres)
//        Increase(profile.Genres, genre, weight);

//    Increase(profile.Years, movie.YearBucket, weight);
//    Increase(profile.Ratings, movie.RatingBucket, weight);

//    foreach (var actor in movie.Actors)
//        Increase(profile.Actors, actor, weight);
//}

//private void Increase(Dictionary<string, double> dict, string key, double value)
//{
//    if (!dict.ContainsKey(key))
//        dict[key] = 0;

//    dict[key] += value;
//}


//public double CalculateScore(UserProfile profile, MovieFeatures movie)
//{
//    double score = 0;

//    foreach (var genre in movie.Genres)
//        if (profile.Genres.ContainsKey(genre))
//            score += profile.Genres[genre];

//    if (profile.Years.ContainsKey(movie.YearBucket))
//        score += profile.Years[movie.YearBucket];

//    if (profile.Ratings.ContainsKey(movie.RatingBucket))
//        score += profile.Ratings[movie.RatingBucket];

//    foreach (var actor in movie.Actors)
//        if (profile.Actors.ContainsKey(actor))
//            score += profile.Actors[actor];

//    return score;
//}


//public MovieFeatures GetRecommendation(
//    UserProfile profile,
//    List<MovieFeatures> movies)
//{
//    const double epsilon = 0.2;
//    var random = new Random();

//    var scored = movies
//        .Select(m => new
//        {
//            Movie = m,
//            Score = CalculateScore(profile, m)
//        })
//        .OrderByDescending(x => x.Score)
//        .ToList();

//    if (random.NextDouble() < epsilon)
//    {
//        // eksploracja – losowy z top 50%
//        int half = scored.Count / 2;
//        return scored[random.Next(half)].Movie;
//    }

//    return scored.First().Movie;
//}

//logEvent(movieId: number, eventType: string) {
//    return this.http.post('/api/recommendation/event', {
//    movieId: movieId,
//    eventType: eventType
//    });
//}


//onPlay(movie: any) {
//    this.recommendationService.logEvent(movie.id, 'play')
//      .subscribe();
//}

//onDetails(movie: any) {
//    this.recommendationService.logEvent(movie.id, 'details')
//      .subscribe();
//}

//onCastClick(movie: any) {
//    this.recommendationService.logEvent(movie.id, 'cast')
//      .subscribe();
//}


