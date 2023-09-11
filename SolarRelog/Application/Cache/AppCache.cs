using Microsoft.Extensions.Caching.Memory;
using SolarRelog.Domain.Entities;

namespace SolarRelog.Application.Cache;

public abstract class AppCache<T>
{
    private readonly IMemoryCache _memCache;
    public abstract string CacheKey { get; }
    
    protected AppCache(IMemoryCache memCache)
    {
        _memCache = memCache;
    }

    public T? GetCache()
    {
        return _memCache.Get<T>(CacheKey);
    }

    public void Cache(T cacheEntry)
    {
        _memCache.Remove(CacheKey);

        _memCache.GetOrCreate(CacheKey, e =>
        {
            e.Priority = CacheItemPriority.NeverRemove;
            return cacheEntry;
        });
    }
}