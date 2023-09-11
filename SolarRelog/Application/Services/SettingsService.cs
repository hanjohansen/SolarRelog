using Microsoft.EntityFrameworkCore;
using SolarRelog.Application.Cache;
using SolarRelog.Domain.Entities;
using SolarRelog.Infrastructure;

namespace SolarRelog.Application.Services;

public class SettingsService
{
    private readonly SettingsCache _cache;
    private readonly DbContextOptions<AppDbContext> _dbContextOptions;

    public SettingsService(SettingsCache cache, DbContextOptions<AppDbContext> dbContextOptions)
    {
        _cache = cache;
        _dbContextOptions = dbContextOptions;
    }

    public async Task<SettingsEntity> GetSettings()
    {
        var settings = _cache.GetCache();

        if (settings != null)
            return settings;

        var context = new AppDbContext(_dbContextOptions);
        settings = await context.Settings.FirstOrDefaultAsync();

        if (settings != null)
        {
            _cache.Cache(settings);
            return settings;
        }
        
        settings = new SettingsEntity();
        _cache.Cache(settings);

        return settings;
    }

    public async Task UpdateSettings(SettingsEntity settings)
    {
        var context = new AppDbContext(_dbContextOptions);
        var existingSettings = await context.Settings.FirstOrDefaultAsync();

        if (existingSettings != null)
            context.Settings.Remove(existingSettings);

        await context.Settings.AddAsync(settings);
        _cache.Cache(settings);

        await context.SaveChangesAsync();
    }
}