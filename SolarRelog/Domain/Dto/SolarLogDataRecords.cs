using System.Text.Json.Serialization;
using SolarRelog.Domain.Entities;

namespace SolarRelog.Domain.Dto;

public class SolarLogRequest{
    [JsonPropertyName("801")]
    public SolarLogRecord? Record {get;set;}
    
    [JsonPropertyName("782")]
    public Dictionary<string, string>? ConsumerData {get;set;}

    public LogDataEntity? ToEntity()
    {
        if (Record == null || Record.Data == null)
            return null;

        var entity = Record.Data.ToEntity();

        if (ConsumerData == null)
            return entity;

        foreach (var index in ConsumerData.Keys)
        {
            var consumption = 0m;
            decimal.TryParse(ConsumerData[index], out consumption);
            
            entity.ConsumerData.Add(new LogConsumerData()
            {
                ConsumerIndex = index,
                Consumption = consumption
            });
        }

        return entity;
    }
};

public class SolarLogRecord{
    [JsonPropertyName("170")]
    public SolarLogData? Data {get;set;}
};

public class SolarLogData
{

    [JsonPropertyName("100")] 
    public string LastUpdateTime { get; set; } = null!; //Date.ToString("dd.MM.yy HH:mm:ss"); //Uhrzeit

    [JsonPropertyName("101")]
    public decimal Pac {get;set;} //in W. Gesamte Leistung PAC von allen Wech-selrichtern und Zählern im Wechselrichtermodus

    [JsonPropertyName("102")]
    public decimal Pdc {get;set;} //in W. Gesamte Leistung PDC von allen Wechselrichtern

    [JsonPropertyName("103")]
    public decimal Uac {get;set;} //in V. Durchschnittliche Spannung UAC der Wechselrichter

    [JsonPropertyName("104")]
    public decimal Udc {get;set;} //in V. Durchschnittliche Spannung UDC der Wechselrichter

    [JsonPropertyName("105")]
    public decimal YieldDay {get;set;} //in Wh. Summierter Tagesertrag aller Wechselrichter

    [JsonPropertyName("106")]
    public decimal YieldYesterday {get;set;} //in Wh. Summierter gestriger Tagesertrag aller Wechselrichter

    [JsonPropertyName("107")]
    public decimal YieldMonth {get;set;} //in Wh. Summierter Monatsertrag aller Wechselrichter

    [JsonPropertyName("108")]
    public decimal YieldYear {get;set;} //in Wh. Summierter Jahresertrag aller Wechselrichter

    [JsonPropertyName("109")]
    public decimal YieldTotal {get;set;} //in Wh. Gesamtertrag aller Wechselrichter

    [JsonPropertyName("110")]
    public decimal ConsPac {get;set;} //in W. Momentaner Gesamtverbrauch PAC aller Verbrauchszähler

    [JsonPropertyName("111")]
    public decimal ConsYieldDay {get;set;} //in Wh. Summierter Verbrauch aller Verbrauchszähler

    [JsonPropertyName("112")]
    public decimal ConsYieldYesterday {get;set;} //in Wh. Summierter Verbrauch des gestrigen Tages; alle Verbrauchszähler

    [JsonPropertyName("113")]
    public decimal ConsYieldMonth {get;set;} //in Wh. Summierter Verbrauch des Monats; alle Verbrauchszähler

    [JsonPropertyName("114")]
    public decimal ConsYieldYear {get;set;} //in Wh. Summierter Verbrauch des Jahres; alle Verbrauchszähler

    [JsonPropertyName("115")]
    public decimal ConsYieldTotal {get;set;} //in Wh. Summierter Gesamtverbrauch; alle Verbrauchszähler

    [JsonPropertyName("116")]
    public decimal TotalPower {get;set;} //in Wp. Summierter Gesamtverbrauch; alle Verbrauchszähler

    public LogDataEntity ToEntity()
    {
        DateTime loggedDate;
        
        if(!DateTime.TryParseExact(LastUpdateTime, "dd.MM.yy HH:mm:ss", null,System.Globalization.DateTimeStyles.None, out loggedDate))
            loggedDate = DateTime.Now;

        return new LogDataEntity
        {
            RecordDate = DateTime.Now,
            LoggedDate = loggedDate,
            Pac = Pac,
            Pdc = Pdc,
            Uac = Uac,
            Udc = Udc,
            YieldDay = YieldDay,
            YieldYesterday = YieldYesterday,
            YieldMonth = YieldMonth,
            YieldYear = YieldYear,
            YieldTotal = YieldTotal,
            ConsPac = ConsPac,
            ConsYieldDay = ConsYieldDay,
            ConsYieldYesterday = ConsYieldYesterday,
            ConsYieldMonth = ConsYieldMonth,
            ConsYieldYear = ConsYieldYear,
            ConsYieldTotal = ConsYieldTotal,
            TotalPower = TotalPower,
        };
    }
}


