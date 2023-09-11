namespace SolarRelog.Domain.Entities;

public class SettingsEntity
{
    public Guid Id { get; set; }

    public AppLogSettings AppLogSettings { get; set; } = new();
}

public class AppLogSettings
{
    public int RetentionDays { get; set; } = 180;

    public LogLevel MinLogLevel { get; set; } = LogLevel.Information;
}