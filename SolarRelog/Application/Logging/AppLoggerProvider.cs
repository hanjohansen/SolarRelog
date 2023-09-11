namespace SolarRelog.Application.Logging;

public class AppLoggerProvider : ILoggerProvider
{
    private readonly IServiceProvider _serviceProvider;
    
    public AppLoggerProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public ILogger CreateLogger(string categoryName)
    {
        return _serviceProvider.GetRequiredService<ILogger>();
    }

    public void Dispose()
    {
    }
}