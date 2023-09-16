using MediatR;
using SolarRelog.Application.ServiceInterfaces;
using SolarRelog.Domain.Entities;

namespace SolarRelog.Application.Queries;

public record GetDevicesQuery : IRequest<List<DeviceEntity>>;

public class GetDevicesQueryHandler : IRequestHandler<GetDevicesQuery, List<DeviceEntity>>
{
    private readonly IDeviceService _devices;

    public GetDevicesQueryHandler(IDeviceService devices)
    {
        _devices = devices;
    }

    public async Task<List<DeviceEntity>> Handle(GetDevicesQuery request, CancellationToken cancellationToken)
    {
        return await _devices.GetAllDevices(cancellationToken);
    }
}