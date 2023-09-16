using Microsoft.EntityFrameworkCore;
using Quartz;
using SolarRelog.Application.Exceptions;
using SolarRelog.Application.Services;
using SolarRelog.Domain.Dto;
using SolarRelog.Domain.Entities;
using SolarRelog.Infrastructure;

namespace SolarRelog.Application.Jobs.LogRecords;

public class LogDataJob : IJob
{
    private readonly SettingsService _settingsService;
    private readonly SolarLogClientService _clientService;
    private readonly AppDbContext _dbContext;
    private readonly ILogger _logger;

    private static readonly JobKey JobKey = new ("request-log-data-job", "log-data");

    public LogDataJob(SettingsService settingsService, AppDbContext dbContext, SolarLogClientService clientService, ILogger logger)
    {
        _settingsService = settingsService;
        _dbContext = dbContext;
        _clientService = clientService;
        _logger = logger;
    }

    public static JobKey Key => JobKey;
    
    public async Task Execute(IJobExecutionContext context)
    {
        await RescheduleJob(context);

        var devices = await _dbContext.Devices
            .AsNoTracking()
            .Where(x => x.IsActive)
            .ToListAsync();

        if (!devices.Any())
        {
            _logger.LogInformation("No active device registered. Stopping to poll");
            await context.Scheduler.PauseJob(JobKey);
            return;
        }
            
        
        foreach (var device in devices)
            await ProcessDevice(device);

        await _dbContext.SaveChangesAsync();
    }

    private async Task ProcessDevice(DeviceEntity device)
    {
        try
        {
            var logData = await GetLogData(device.Ip, device.Port);

            var newEntity = logData.ToEntity()!;
            newEntity.DeviceId = device.Id;

            await _dbContext.Logs.AddAsync(newEntity);
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, ex.Message, ex.InnerException?.Message);
        }
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