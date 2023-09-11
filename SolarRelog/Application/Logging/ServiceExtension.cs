using Microsoft.EntityFrameworkCore;
using SolarRelog.Application.Logging.Data;

namespace SolarRelog.Application.Logging;

public static class ServiceExtension
{
    public static WebApplicationBuilder ConfigureAppLogging(this WebApplicationBuilder builder)
    {
        var connString = builder.Configuration.GetValue<string>("ConnectionStrings:logDb");
        
        builder.Services.AddScoped<ILogger, AppLogger>();
        builder.Services.AddDbContext<LogDbContext>(opt 
                => opt.UseSqlite(connString));
        
        var serviceProvider = builder.Services.BuildServiceProvider();
        builder.Logging
            .ClearProviders()
            .AddProvider(new AppLoggerProvider(serviceProvider));

        var client = serviceProvider.GetRequiredService<LogDbContext>();
        client.Database.EnsureCreated();

        return builder;
    }
}