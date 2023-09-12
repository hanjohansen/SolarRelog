using Microsoft.EntityFrameworkCore;
using Quartz;
using SolarRelog.Application.Logging.Data;
using SolarRelog.Application.Services;

namespace SolarRelog.Application.Jobs.Logs;

public class LogCleanupJob : ISolarRelogJob
{
    private readonly ILogger _logger;
    private readonly LogDbContext _logContext;
    private readonly SettingsService _settingsService;
    
    private const string KeyName = "clean-logs-job";
    private const string KeyGroup = "logs";

    public LogCleanupJob(ILogger logger, LogDbContext logContext, SettingsService settingsService)
    {
        _logger = logger;
        _logContext = logContext;
        _settingsService = settingsService;
    }
    
    public static JobKey JobKey => new (KeyName, KeyGroup);
    
    public async Task Execute(IJobExecutionContext context)
    {
        await RescheduleJob(context);
        
        var settings = await _settingsService.GetSettings();
        var retentionDays = settings.AppLogSettings.RetentionDays;
        var refDate = DateTime.Now.AddDays(-retentionDays);

        _logger.LogInformation($"Executing log-cleanup with {retentionDays} retention days");
        
        await _logContext.Logs.Where(x => x.TimeStamp < refDate)
            .ExecuteDeleteAsync();
    }

    private async Task RescheduleJob(IJobExecutionContext context)
    {
        await context.Scheduler.UnscheduleJob(context.Trigger.Key);
        
        var newTrigger = TriggerBuilder.Create()
            .ForJob(JobKey)
            .StartAt(DateTimeOffset.UtcNow.AddDays(1))
            .Build();
        
        await context.Scheduler.ScheduleJob(newTrigger);
    }
}