using MediatR;
using Microsoft.EntityFrameworkCore;
using SolarRelog.Domain.Entities;
using SolarRelog.Infrastructure;

namespace SolarRelog.Application.Queries;

public record GetDevicesQuery : IRequest<List<DeviceEntity>>;

public class GetDevicesQueryHandler : IRequestHandler<GetDevicesQuery, List<DeviceEntity>>
{
    private readonly AppDbContext _dbContext;

    public GetDevicesQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<DeviceEntity>> Handle(GetDevicesQuery request, CancellationToken cancellationToken)
    {
        var devices = await _dbContext.Devices
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return devices;
    }
}