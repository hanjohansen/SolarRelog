using MediatR;
using Quartz;
using SolarRelog.Application.Exceptions;
using SolarRelog.Application.Jobs.LogRecords;
using SolarRelog.Application.Services;
using SolarRelog.Domain.Dto;
using SolarRelog.Domain.Entities;

namespace SolarRelog.Application.Commands.Settings;

public record UpdateSettingsCommand(SettingsModel Payload) : IRequest;

public class UpdateSettingsCommandHandler : IRequestHandler<UpdateSettingsCommand>
{
    private readonly ILogger _logger;
    private readonly SettingsService _settingsService;
    private readonly ISchedulerFactory _schedulerFactory;
    
    public UpdateSettingsCommandHandler(SettingsService settingsService, ILogger logger, ISchedulerFactory schedulerFactory)
    {
        _settingsService = settingsService;
        _logger = logger;
        _schedulerFactory = schedulerFactory;
    }

    public async Task Handle(UpdateSettingsCommand request, CancellationToken cancellationToken)
    {
        var model = request.Payload;
        ValidateModel(model);
        var oldSettings = await _settingsService.GetSettings();

        var settings = new SettingsEntity
        {
            AppLogSettings = new AppLogSettings
            {
                RetentionDays = model.LogSettings.RetentionDays,
                MinLogLevel = model.LogSettings.MinLogLevel
            },
            DataLogSettings = new DataLogSettings
            {
                RetentionDays = model.DataLogSettings.RetentionDays,
                PollingIntervalSeconds = model.DataLogSettings.PollingIntervalSeconds
            },
            InfluxSettings = new InfluxSettings()
            {
                Url = model.InfluxSettings.Url,
                Organization = model.InfluxSettings.Organization,
                Bucket = model.InfluxSettings.Bucket,
                ApiToken = model.InfluxSettings.ApiToken
            }
        };

        await _settingsService.UpdateSettings(settings);
        
        _logger.LogInformation("Settings updated by user");

        if (oldSettings.DataLogSettings.PollingIntervalSeconds !=
            settings.DataLogSettings.PollingIntervalSeconds)
        {
            _logger.LogInformation("Data polling interval changed. Rescheduling polling");
            await ReschedulePollingJob(_schedulerFactory, settings.DataLogSettings.PollingIntervalSeconds);
        }
    }

    private static void ValidateModel(SettingsModel model)
    {
        if(model.LogSettings.RetentionDays is < 1 or > 365)
            ThrowException("LogSettings.RetentionDays: only 1 to 365 days are supported");

        if (model.DataLogSettings.RetentionDays is < -1 or 0)
            ThrowException("DataLogSettings.RetentionDays: only -1 (disabled) 1+ days are supported");
        
        if (model.DataLogSettings.PollingIntervalSeconds is < 15)
            ThrowException("DataLogSettings.PollingIntervalSeconds: min. interval length is 15s");
    }

    private static void ThrowException(string message)
    {
        throw new AppException(message);
    }

    private async Task ReschedulePollingJob(ISchedulerFactory schedulerFactory, int newInterval)
    {
        var scheduler = await schedulerFactory.GetScheduler();
        var triggers = await scheduler.GetTriggersOfJob(LogDataJob.Key);

        var newTrigger = TriggerBuilder
            .Create()
            .ForJob(LogDataJob.Key)
            .StartAt(DateTimeOffset.UtcNow.AddSeconds(newInterval))
            .Build();
        
        foreach (var trigger in triggers)
            await scheduler.RescheduleJob(trigger.Key, newTrigger);
    }
} 