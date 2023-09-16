namespace SolarRelog.Application.Services;

public static class ServiceExtension
{
    public static WebApplicationBuilder ConfigureAppServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<SettingsService>();
        builder.Services.AddScoped<SolarLogClientService>();

        return builder;
    }
}