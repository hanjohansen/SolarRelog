using SolarRelog.DevDevice.Domain;

namespace SolarRelog.DevDevice.Service;

public class SolarLogRecordService
{
    private readonly SolarLogRecord _record;
    private readonly SolarLogConsumers _consumers;

    public SolarLogRecordService(){
        var date = DateTime.Now;

        _record = new SolarLogRecord{
            Data = new SolarLogData{
                Date = date,
                Pac = 300m,
                Pdc = 300m,
                Uac = 500m,
                Udc = 500m,
                YieldDay = 2m,
                YieldYesterday = 150,
                YieldMonth = 150 * date.Day,
                YieldYear = 150 * 30 * (date.Month - 1) + 150 * date.Day,
                YieldTotal = 4 * (150 * 30 * (date.Month - 1) + 150 * date.Day),
                ConsPac = 600,
                ConsYieldDay = 3.5m,
                ConsYieldMonth = 300 * date.Day,
                ConsYieldYesterday = 300,
                ConsYieldYear = 300 * 30 * (date.Month - 1) + 300 * date.Day,
                ConsYieldTotal = 4 * (300 * 30 * (date.Month - 1) + 300 * date.Day)
            }
        };

        var rand = new Random();
        _consumers = new SolarLogConsumers
        {
            Consumer1 = rand.Next(500, 2000).ToString(),
            Consumer2 = rand.Next(500, 2000).ToString(),
            Consumer3 = rand.Next(500, 2000).ToString(),
            Consumer4 = rand.Next(500, 2000).ToString()
        };
    }

    public SolarLogRequest GetLogRecord(bool includeRecord, bool includeConsumers){
        var request = new SolarLogRequest
        {
            Record = includeRecord ? _record : null,
            Consumers = includeConsumers ? _consumers : null
        };
        return request;
    }

    public void UpdateData(){
        var newDate = DateTime.Now;
        var rand = new Random();
        var pcDiff = 300 + rand.Next(-10, 10);
        var ucDiff = 500 + rand.Next(-20, 20);
        var consDiff = 600 + rand.Next(-40, 40);

        _record.Data!.Pac = pcDiff;
        _record.Data!.Pdc = pcDiff;
        _record.Data!.Uac = ucDiff;
        _record.Data!.Udc = ucDiff;

        _record.Data!.YieldDay += .5m;
        _record.Data!.YieldMonth += .5m;
        _record.Data!.YieldYear += .5m;
        _record.Data!.YieldTotal += .5m;

        _record.Data!.ConsPac = consDiff;
        _record.Data!.ConsYieldDay += .8m;
        _record.Data!.ConsYieldMonth += .8m;
        _record.Data!.ConsYieldYear += .8m;

        if(newDate.Day != _record.Data.Date.Day){
            _record.Data.YieldYesterday = _record.Data.YieldDay;
            _record.Data.YieldDay = 0;
        }

        if(newDate.Month != _record.Data.Date.Month){
            _record.Data.YieldMonth = 0;
        }

        if(newDate.Year != _record.Data.Date.Year){
            _record.Data.YieldYear = 0;
        }

        _record.Data.Date = newDate;
        _consumers.Consumer1 = rand.Next(500, 2000).ToString();
        _consumers.Consumer2 = rand.Next(500, 2000).ToString();
        _consumers.Consumer3 = rand.Next(500, 2000).ToString();
        _consumers.Consumer4 = rand.Next(500, 2000).ToString();
    }
}
