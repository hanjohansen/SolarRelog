using System.Net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quartz;
using SolarRelog.Application.Exceptions;
using SolarRelog.Domain.Entities;
using SolarRelog.Infrastructure;

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
    private readonly AppDbContext _dbContext;
    private readonly ILogger _logger;
    private readonly ISchedulerFactory _schedulerFactory;

    public AddDeviceCommandHandler(AppDbContext dbContext, ILogger logger, ISchedulerFactory schedulerFactory)
    {
        _dbContext = dbContext;
        _logger = logger;
        _schedulerFactory = schedulerFactory;
    }

    public async Task Handle(AddDeviceCommand request, CancellationToken cancellationToken)
    {
        request.Validate();

        var existing = await _dbContext.Devices
            .FirstOrDefaultAsync(x => x.Ip == request.Ip, cancellationToken);

        if (existing != null)
            throw new AppException("Provided Ip is already used by an existing device");

        var newDevice = new DeviceEntity
        {
            Name = request.Name,
            Ip = request.Ip,
            Port = request.Port,
            IsActive = request.IsActive
        };

        await _dbContext.Devices.AddAsync(newDevice, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation($"Created device for Ip '{newDevice.Ip}'");

        await UnpausePollingJob(_dbContext, _schedulerFactory, _logger);
    }
}
    
    