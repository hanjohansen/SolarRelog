namespace SolarRelog.Domain.Entities;

public class LogDataEntity
{
    public Guid Id { get; set; }
    
    public Guid DeviceId { get; set; }

    public DeviceEntity Device { get; set; } = null!;

    public List<LogConsumerData> ConsumerData { get; set; } = new();
    
    public DateTime RecordDate { get; set; } 
    
    public DateTime LoggedDate {get;set;}
    
    public decimal Pac {get;set;} 
    
    public decimal Pdc {get;set;} 
    
    public decimal Uac {get;set;}
    
    public decimal Udc {get;set;}
    
    public decimal YieldDay {get;set;} 
    
    public decimal YieldYesterday {get;set;} 
    
    public decimal YieldMonth {get;set;}
    
    public decimal YieldYear {get;set;}
    
    public decimal YieldTotal {get;set;}
    
    public decimal ConsPac {get;set;}
    
    public decimal ConsYieldDay {get;set;}
    
    public decimal ConsYieldYesterday {get;set;}
    
    public decimal ConsYieldMonth {get;set;}
    
    public decimal ConsYieldYear {get;set;}
    
    public decimal ConsYieldTotal {get;set;}
    
    public decimal TotalPower {get;set;} 
}