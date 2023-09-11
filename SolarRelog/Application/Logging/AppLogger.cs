using Newtonsoft.Json;
using SolarRelog.Application.Logging.Data;

namespace SolarRelog.Application.Logging;

public class AppLogger : ILogger
{
    private readonly LogDbContext _dbContext;
    
    public AppLogger(LogDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default!;

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
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
