using Quartz;
using SolarRelog.Application.Exceptions;
using SolarRelog.Application.ServiceInterfaces;
using SolarRelog.Application.Services;
using SolarRelog.Domain.Dto;
using SolarRelog.Domain.Entities;

namespace SolarRelog.Application.Jobs.LogRecords;

public class LogDataJob : IJob
{
    private readonly IDeviceService _devices;
    private readonly ILogDataService _logData;
    private readonly SettingsService _settingsService;
    private readonly SolarLogClientService _clientService;
    private readonly ILogger _logger;

    private static readonly JobKey JobKey = new ("request-log-data-job", "log-data");

    public LogDataJob(SettingsService settingsService, SolarLogClientService clientService, ILogger logger, IDeviceService devices, ILogDataService logData)
    {
        _settingsService = settingsService;
        _clientService = clientService;
        _logger = logger;
        _devices = devices;
        _logData = logData;
    }

    public static JobKey Key => JobKey;
    
    public async Task Execute(IJobExecutionContext context)
    {
        await RescheduleJob(context);

        var devices = await _devices.GetActiveDevices();

        if (!devices.Any())
        {
            _logger.LogInformation("No active device registered. Stopping to poll");
            await context.Scheduler.PauseJob(JobKey);
            return;
        }

        var logs = new List<LogDataEntity>();

        foreach (var device in devices)
        {
            var log = await ProcessDevice(device);
            if(log != null)
                logs.Add(log);
        }

        await _logData.AddLogData(logs);
    }

    private async Task<LogDataEntity?> ProcessDevice(DeviceEntity device)
    {
        try
        {
            var logData = await GetLogData(device.Ip, device.Port);

            var newEntity = logData.ToEntity()!;
            newEntity.DeviceId = device.Id;

            return newEntity;
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, ex.Message, ex.InnerException?.Message);
        }

        return null;
    }

    private async Task<SolarLogRequest> GetLogData(string ip, int? port)
    {
        try
        {
            var logData = await _clientService.RequestLogData(ip, port);
            
            if (logData.Record?.Data == null)
                throw new AppException($"Device did not return any data");

            return logData;

        }
        catch (Exception ex)
        {
            throw new AppException($"Error retrieving log data from device with IP '{ip}'", ex);
        }
    }
    
    private async Task RescheduleJob(IJobExecutionContext context)
    {
        var settings = await _settingsService.GetSettings();
        
        var newTrigger = TriggerBuilder.Create()
            .ForJob(JobKey)
            .StartAt(DateTimeOffset.UtcNow.AddSeconds(settings.DataLogSettings.PollingIntervalSeconds))
            .Build();
        
        await context.Scheduler.RescheduleJob(context.Trigger.Key, newTrigger);
    }
}