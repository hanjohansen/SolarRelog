using MediatR;
using Microsoft.EntityFrameworkCore;
using SolarRelog.Application.Exceptions;
using SolarRelog.Infrastructure;

namespace SolarRelog.Application.Commands.Device;

public record UpdateDeviceCommand(
    Guid Id,
    string Name,
    string Ip,
    int? Port,
    bool IsActive) : AddDeviceCommand(Name, Ip, Port, IsActive);

public class UpdateDeviceCommandHandler : IRequestHandler<UpdateDeviceCommand>
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger _logger;

    public UpdateDeviceCommandHandler(AppDbContext dbContext, ILogger logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Handle(UpdateDeviceCommand request, CancellationToken cancellationToken)
    {
        request.Validate();

        var device = await _dbContext.Devices
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (device == null)
            throw new EntityNotFoundException($"No device with id '{request.Id}' found");

        var oldIp = device.Ip;
        
        device.Name = request.Name;
        device.Ip = request.Ip;
        device.Port = request.Port;
        device.IsActive = request.IsActive;
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(device.Ip == oldIp
            ? $"Updated device with Ip '{device.Ip}'"
            : $"Updated device with Ip (old/new) '{oldIp}/{device.Ip}'");
    }
}
    
    