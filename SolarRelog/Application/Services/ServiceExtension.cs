using SolarRelog.Application.ServiceInterfaces;

namespace SolarRelog.Application.Services;

public static class ServiceExtension
{
    public static WebApplicationBuilder ConfigureAppServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IDeviceService, DeviceService>();
        builder.Services.AddScoped<ILogDataService, LogDataService>();
        
        builder.Services.AddScoped<SettingsService>();
        builder.Services.AddScoped<SolarLogClientService>();
        builder.Services.AddScoped<IInfluxService, InfluxService>();

        return builder;
    }
}