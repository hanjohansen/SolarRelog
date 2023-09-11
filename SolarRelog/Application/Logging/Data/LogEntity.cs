namespace SolarRelog.Application.Logging.Data;

public class LogEntity
{
    public Guid Id { get; set; }
    
    public DateTime TimeStamp { get; set; }

    public LogLevel Level { get; set; }
    
    public string Message { get; set; } = null!;
    
    public string? Payload { get; set; }
}