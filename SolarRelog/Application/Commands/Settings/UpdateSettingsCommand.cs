using MediatR;
using SolarRelog.Application.Exceptions;
using SolarRelog.Application.Services;
using SolarRelog.Domain.Dto;
using SolarRelog.Domain.Entities;

namespace SolarRelog.Application.Commands.Settings;

public record UpdateSettingsCommand(SettingsModel Payload) : IRequest;

public class UpdateSettingsCommandHandler : IRequestHandler<UpdateSettingsCommand>
{
    private readonly ILogger _logger;
    private readonly SettingsService _settingsService;
    
    public UpdateSettingsCommandHandler(SettingsService settingsService, ILogger logger)
    {
        _settingsService = settingsService;
        _logger = logger;
    }

    public async Task Handle(UpdateSettingsCommand request, CancellationToken cancellationToken)
    {
        var model = request.Payload;
        ValidateModel(model);

        var settings = new SettingsEntity
        {
            AppLogSettings = new AppLogSettings
            {
                RetentionDays = model.LogSettings.RetentionDays,
                MinLogLevel = model.LogSettings.MinLogLevel
            }
        };

        await _settingsService.UpdateSettings(settings);
        
        _logger.LogInformation("Settings updated by user");
    }

    private static void ValidateModel(SettingsModel model)
    {
        if(model.LogSettings.RetentionDays is < 1 or > 365)
            ThrowException("LogSettings.RetentionDays: only 1 to 365 days are supported");
    }

    private static void ThrowException(string message)
    {
        throw new AppException(message);
    }
} 