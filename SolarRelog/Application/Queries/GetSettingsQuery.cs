using MediatR;
using SolarRelog.Application.Services;
using SolarRelog.Domain.Dto;

namespace SolarRelog.Application.Queries;

public record GetSettingsQuery : IRequest<SettingsModel>;

public class SettingsQueryHandler : IRequestHandler<GetSettingsQuery, SettingsModel>
{
    private readonly SettingsService _settingsService;

    public SettingsQueryHandler(SettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    public async Task<SettingsModel> Handle(GetSettingsQuery request, CancellationToken cancellationToken)
    {
        var settings = await _settingsService.GetSettings();

        return new SettingsModel(settings.AppLogSettings, settings.DataLogSettings);
    }
}