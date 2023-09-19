using Quartz;
using SolarRelog.Application.ServiceInterfaces;
using SolarRelog.Application.Services;

namespace SolarRelog.Application.Jobs.LogRecords;

public class LogDataCleanUpJob : IJob
{
    private readonly ILogger _logger;
    private readonly SettingsService _settings;
    private readonly ILogDataService _logData;
    
    private static readonly JobKey JobKey = new ("cleanup-log-data-job", "log-data");

    public LogDataCleanUpJob(SettingsService settings, ILogDataService logData, ILogger logger)
    {
        _settings = settings;
        _logData = logData;
        _logger = logger;
    }

    public static JobKey Key => JobKey;
    
    public async Task Execute(IJobExecutionContext context)
    {
        await RescheduleJob(context);

        var settings = await _settings.GetSettings();
        var logDataSettings = settings.DataLogSettings;

        if (logDataSettings.RetentionDays == -1)
            return;
        
        _logger.LogInformation($"Executing data logs cleanup with {logDataSettings.RetentionDays} retention days");

        var refDate = DateTime.UtcNow.AddDays(-logDataSettings.RetentionDays);
        await _logData.DeleteLogDataAfter(refDate);
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