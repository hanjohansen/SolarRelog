using SolarRelog.Domain.Entities;

namespace SolarRelog.Application.ServiceInterfaces;

public interface IDeviceService
{
    Task<DeviceEntity> GetDeviceById(Guid id, bool trackEntity, CancellationToken ct = default);
    
    Task<DeviceEntity?> GetDeviceByIp(string ip, CancellationToken ct = default);
    
    Task<List<DeviceEntity>> GetAllDevices(CancellationToken ct = default);
    
    Task<List<DeviceEntity>> GetActiveDevices(CancellationToken ct = default);

    Task AddDevice(DeviceEntity newEntity, CancellationToken ct = default);

    Task UpdateEntity(Guid id, string name, string ip, int? port, bool isActive, CancellationToken ct = default);

    Task DeleteDevice(Guid deviceId, CancellationToken ct = default);
}