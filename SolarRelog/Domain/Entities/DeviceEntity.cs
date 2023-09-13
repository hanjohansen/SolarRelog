namespace SolarRelog.Domain.Entities;

public class DeviceEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Ip { get; set; } = null!;
    
    public int? Port { get; set; }
    
    public bool IsActive { get; set; }

    public List<LogDataEntity> LogData { get; set; } = new();
}