﻿namespace SolarRelog.Domain.Entities;

public class LogConsumerData
{
    public Guid Id { get; set; }
    
    public Guid LogDataId { get; set; }

    public LogDataEntity LogData { get; set; } = null!;
    
    public int ConsumerIndex { get; set; }
    
    public decimal Consumption { get; set; }
}