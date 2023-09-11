namespace SolarRelog.Application.Cache;

public static class ServiceExtensions
{
    public static WebApplicationBuilder ConfigureAppCaching(this WebApplicationBuilder builder)
    {
        builder.Services.AddMemoryCache();

        builder.Services.AddScoped<SettingsCache>();

        return builder;
    }
}