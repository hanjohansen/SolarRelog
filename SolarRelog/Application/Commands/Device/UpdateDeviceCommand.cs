using MediatR;
using Quartz;
using SolarRelog.Application.ServiceInterfaces;

namespace SolarRelog.Application.Commands.Device;

public record UpdateDeviceCommand(
    Guid Id,
    string Name,
    string Ip,
    int? Port,
    bool IsActive) : AddDeviceCommand(Name, Ip, Port, IsActive);

public class UpdateDeviceCommandHandler : BaseDeviceCommandHandler, IRequestHandler<UpdateDeviceCommand>
{
    private readonly ILogger _logger;
    private readonly IDeviceService _devices;
    private readonly ISchedulerFactory _schedulerFactory;

    public UpdateDeviceCommandHandler(ILogger logger, ISchedulerFactory schedulerFactory, IDeviceService devices)
    {
        _logger = logger;
        _schedulerFactory = schedulerFactory;
        _devices = devices;
    }

    public async Task Handle(UpdateDeviceCommand request, CancellationToken cancellationToken)
    {
        request.Validate();

        await _devices.UpdateEntity(
            request.Id,
            request.Name,
            request.Ip,
            request.Port,
            request.IsActive,
            cancellationToken);

        _logger.LogInformation($"Updated device with Id '{request.Id}'");

        await UnpausePollingJob(_devices, _schedulerFactory, _logger); 
    }
}
    
    