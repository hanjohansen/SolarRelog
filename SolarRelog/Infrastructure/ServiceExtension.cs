using Microsoft.EntityFrameworkCore;

namespace SolarRelog.Infrastructure;

public static class ServiceExtension
{
    public static WebApplicationBuilder ConfigureAppDatabase(this WebApplicationBuilder builder)
    {
        var connString = builder.Configuration.GetValue<string>("ConnectionStrings:appDb");
        
        builder.Services.AddDbContext<AppDbContext>(opt 
                => opt.UseSqlite(connString));
        
        var serviceProvider = builder.Services.BuildServiceProvider();

        var client = serviceProvider.GetRequiredService<AppDbContext>();
        client.Database.EnsureCreated();
        client.Database.ExecuteSql($"PRAGMA journal_mode = 'delete';");

        return builder;
    }
}