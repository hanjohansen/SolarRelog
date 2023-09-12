using MediatR;
using Microsoft.EntityFrameworkCore;
using SolarRelog.Application.Exceptions;
using SolarRelog.Infrastructure;

namespace SolarRelog.Application.Commands.Device;

public record DeleteDeviceCommand(Guid Id) : IRequest;

public class DeleteDeviceCommandHandler : IRequestHandler<DeleteDeviceCommand>
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger _logger;

    public DeleteDeviceCommandHandler(AppDbContext dbContext, ILogger logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Handle(DeleteDeviceCommand request, CancellationToken cancellationToken)
    {
        var device = await _dbContext.Devices
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (device == null)
            throw new EntityNotFoundException($"No device with id '{request.Id}' found");

        _dbContext.Devices.Remove(device);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation($"Removed device with Ip {device.Ip}");
    }
}
    
    