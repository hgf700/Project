//using Microsoft.Extensions.Caching.Memory;
//using ProjectBackend.Models.ReleatedToMovie;

//IMemoryCache _cache;

//if (!_cache.TryGetValue(movieId, out List<string> actors))
//{
//    actors = await GetTopActorsAsync(movieId);
//    _cache.Set(movieId, actors, TimeSpan.FromDays(7));
//}
