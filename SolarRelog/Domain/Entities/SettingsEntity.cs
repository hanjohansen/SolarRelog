namespace SolarRelog.Domain.Entities;

public class SettingsEntity
{
    public Guid Id { get; set; }

    public AppLogSettings AppLogSettings { get; set; } = new();
    
    public DataLogSettings DataLogSettings { get; set; } = new();
}

public class AppLogSettings
{
    public int RetentionDays { get; set; } = 30;

    public LogLevel MinLogLevel { get; set; } = LogLevel.Information;
}

public class DataLogSettings
{
    public int RetentionDays { get; set; } = -1;

    public int PollingIntervalSeconds { get; set; } = 30;
}