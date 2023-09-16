using MediatR;
using SolarRelog.Application.ServiceInterfaces;

namespace SolarRelog.Application.Commands.Device;

public record DeleteDeviceCommand(Guid Id) : IRequest;

public class DeleteDeviceCommandHandler : IRequestHandler<DeleteDeviceCommand>
{
    private readonly ILogger _logger;
    private readonly IDeviceService _devices;

    public DeleteDeviceCommandHandler(ILogger logger, IDeviceService devices)
    {
        _logger = logger;
        _devices = devices;
    }

    public async Task Handle(DeleteDeviceCommand request, CancellationToken cancellationToken)
    {
        await _devices.DeleteDevice(request.Id, cancellationToken); 
        
        _logger.LogInformation($"Removed device with Id {request.Id}");
    }
}
    
    