using System.Text.Json.Serialization;

namespace SolarRelog.Domain.Entities;

public class LogConsumerData
{
    [JsonIgnore]
    public Guid Id { get; set; }
    
    [JsonIgnore]
    public Guid LogDataId { get; set; }

    [JsonIgnore]
    public LogDataEntity LogData { get; set; } = null!;

    public string ConsumerIndex { get; set; } = null!;
    
    public decimal Consumption { get; set; }
}