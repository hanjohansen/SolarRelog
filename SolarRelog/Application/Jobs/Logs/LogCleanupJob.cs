using Microsoft.EntityFrameworkCore;
using Quartz;
using SolarRelog.Application.Logging.Data;
using SolarRelog.Application.Services;

namespace SolarRelog.Application.Jobs.Logs;

public class LogCleanupJob : IJob
{
    private readonly ILogger _logger;
    private readonly LogDbContext _logContext;
    private readonly SettingsService _settingsService;
    
    private static readonly JobKey JobKey = new ("clean-logs-job", "logs");

    public LogCleanupJob(ILogger logger, LogDbContext logContext, SettingsService settingsService)
    {
        _logger = logger;
        _logContext = logContext;
        _settingsService = settingsService;
    }

    public static JobKey Key => JobKey;

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

    private static async Task RescheduleJob(IJobExecutionContext context)
    {
        var newTrigger = TriggerBuilder.Create()
            .ForJob(JobKey)
            .StartAt(DateTimeOffset.UtcNow.AddDays(1))
            .Build();
        
        await context.Scheduler.RescheduleJob(context.Trigger.Key, newTrigger);
    }
}