using Microsoft.Extensions.Caching.Memory;
using SolarRelog.Domain.Entities;

namespace SolarRelog.Application.Cache;

public class SettingsCache : AppCache<SettingsEntity>
{
    public SettingsCache(IMemoryCache memCache) : base(memCache)
    {
    }

    public override string CacheKey => "settings";
}