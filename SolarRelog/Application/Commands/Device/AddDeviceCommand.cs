using System.Net;
using MediatR;
using Quartz;
using SolarRelog.Application.Exceptions;
using SolarRelog.Application.ServiceInterfaces;
using SolarRelog.Domain.Entities;

namespace SolarRelog.Application.Commands.Device;

public record AddDeviceCommand(
    string Name,
    string Ip,
    int? Port,
    bool IsActive) : IRequest
{
    public void Validate()
    {
        if(string.IsNullOrEmpty(Name))
            Throw("Device name must be set");
        
        if(string.IsNullOrEmpty(Ip))
            Throw("Device ip must be set");
        
        if (IPAddress.TryParse(Ip, out var address))
        {
            switch (address.AddressFamily)
            {
                case System.Net.Sockets.AddressFamily.InterNetwork:
                    break;
                case System.Net.Sockets.AddressFamily.InterNetworkV6:
                default:
                    Throw("Device ip format is not supported");
                    break;
            }
        }
        else
            Throw("Device IP is not valid");

        if(Port is < 0 or > 65535)
            Throw("Device port is not valid");
    }

    private static void Throw(string message)
    {
        throw new AppException(message);
    }
}

public class AddDeviceCommandHandler : BaseDeviceCommandHandler, IRequestHandler<AddDeviceCommand>
{
    private readonly ILogger _logger;
    private readonly IDeviceService _devices;
    private readonly ISchedulerFactory _schedulerFactory;

    public AddDeviceCommandHandler(ILogger logger, ISchedulerFactory schedulerFactory, IDeviceService devices)
    {
        _logger = logger;
        _schedulerFactory = schedulerFactory;
        _devices = devices;
    }

    public async Task Handle(AddDeviceCommand request, CancellationToken ct)
    {
        request.Validate();

        var existing = await _devices.GetAllDevices(ct);

        if (existing.Any(x => x.Ip == request.Ip))
            throw new AppException("Provided Ip is already used by an existing device");
        
        if (existing.Any(x => x.Name == request.Name))
            throw new AppException("Provided name is already used by an existing device");

        var newDevice = new DeviceEntity
        {
            Name = request.Name,
            Ip = request.Ip,
            Port = request.Port,
            IsActive = request.IsActive
        };

        await _devices.AddDevice(newDevice, ct);
        
        _logger.LogInformation($"Created device for Ip '{newDevice.Ip}'");

        await UnpausePollingJob(_devices, _schedulerFactory, _logger);
    }
}
    
    