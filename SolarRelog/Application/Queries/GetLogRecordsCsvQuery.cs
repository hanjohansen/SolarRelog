using System.Collections;
using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using MediatR;
using SolarRelog.Application.ServiceInterfaces;
using SolarRelog.Domain.Entities;

namespace SolarRelog.Application.Queries;

public record FileData(Stream Content, string ContentType, string FileName);
public record LoggedData(
    DateTime Logged,
    decimal Pac,
    decimal Pdc,
    decimal Uac,
    decimal Udc,
    decimal YieldDay,
    decimal YieldYesterday,
    decimal YieldMonth,
    decimal ConsPac,
    decimal ConsYieldDay,
    decimal ConsYieldYesterday,
    decimal ConsYieldMonth,
    string? ConsumerIndex,
    decimal? Consumption);
public record GetLogRecordsCsvQuery(Guid DeviceId, int Year, int Month) : IRequest<FileData>;

public class GetLogRecordsCsvQueryHandler : IRequestHandler<GetLogRecordsCsvQuery, FileData>
{
    private readonly ILogDataService _logData;

    public GetLogRecordsCsvQueryHandler(ILogDataService logData)
    {
        _logData = logData;
    }

    public async Task<FileData> Handle(GetLogRecordsCsvQuery request, CancellationToken cancellationToken)
    {
        var entities =  await _logData.GetLogData(request.DeviceId, request.Year, request.Month, cancellationToken);
        var result = new List<LoggedData>();
        
        foreach (var entity in entities)
        {
            if(entity.ConsumerData.Any())
                foreach(var consumer in entity.ConsumerData)
                    result.Add(FromConsumer(entity, consumer));
            else
                result.Add(FromMeasurement(entity));
        }
        
        var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ";",
            Encoding = Encoding.UTF8
        };

        var mem = new MemoryStream();
        var writer = new StreamWriter(mem);
        var csvWriter = new CsvWriter(writer, csvConfig);
        
        await csvWriter.WriteRecordsAsync((IEnumerable)result, cancellationToken);
        await writer.FlushAsync();

        var timeStamp = DateTime.Now.ToString("__yyyy_MM_dd__HH:mm");
        return new FileData(mem, "application/csv", "LogData_"+request.Year+"_"+request.Month + timeStamp + ".csv");
    }

    private LoggedData FromMeasurement(LogDataEntity entity)
    {
        return new LoggedData(
            entity.RecordDate,
            entity.Pac,
            entity.Pdc,
            entity.Uac,
            entity.Udc,
            entity.YieldDay,
            entity.YieldYesterday,
            entity.YieldMonth,
            entity.ConsPac,
            entity.ConsYieldDay,
            entity.ConsYieldYesterday,
            entity.ConsYieldMonth,
            null,
            null);
    }

    private LoggedData FromConsumer(LogDataEntity entity, LogConsumerData consumer)
    {
        var raw = FromMeasurement(entity);

        return raw with
        {
            ConsumerIndex = consumer.ConsumerIndex,
            Consumption = consumer.Consumption
        };
    }
}