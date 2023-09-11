using Newtonsoft.Json;
using SolarRelog.Application.Logging.Data;
using SolarRelog.Application.Services;

namespace SolarRelog.Application.Logging;

public class AppLogger : ILogger
{
    private readonly SettingsService _settingsService;
    private readonly LogDbContext _dbContext;

    public AppLogger(SettingsService settingsService, LogDbContext dbContext)
    {
        _settingsService = settingsService;
        _dbContext = dbContext;
    }
    
    public IDisposable BeginScope<TState>(TState state) where TState : notnull => default!;

    public bool IsEnabled(LogLevel logLevel)
    {
        var settings = _settingsService.GetSettings().GetAwaiter().GetResult();

        return logLevel >= settings.AppLogSettings.MinLogLevel;
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception, string>? formatter
    ){

        var message = state?.ToString() ?? string.Empty;
        string? payload = exception != null
            ? JsonConvert.SerializeObject(exception)
            : null;

        var logRecord = new LogEntity()
        {
            TimeStamp = DateTime.Now,
            Level = logLevel,
            Message = message,
            Payload = payload
        };

        _dbContext.Logs.Add(logRecord);
        _dbContext.SaveChanges();
        
        if(logLevel >= LogLevel.Information)
            Console.WriteLine($"{logLevel.ToString()} - {eventId.Id} - {message}");
    }
}
