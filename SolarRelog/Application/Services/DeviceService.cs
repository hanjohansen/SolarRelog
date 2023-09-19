using Microsoft.EntityFrameworkCore;
using SolarRelog.Application.Exceptions;
using SolarRelog.Application.ServiceInterfaces;
using SolarRelog.Domain.Entities;
using SolarRelog.Infrastructure;

namespace SolarRelog.Application.Services;

public class DeviceService : IDeviceService
{
    private readonly AppDbContext _dbContext;

    public DeviceService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DeviceEntity> GetDeviceById(Guid id, bool trackEntity = false, CancellationToken ct = default)
    {
        var entityQuery = _dbContext.Devices.AsQueryable();

        if (!trackEntity)
            entityQuery = entityQuery.AsNoTracking();

        var entity = await entityQuery.FirstOrDefaultAsync(x => x.Id == id, ct);

        if (entity == null)
            throw new EntityNotFoundException($"No device with id '{id}' found");

        return entity;
    }
    
    public async Task<DeviceEntity?> GetDeviceByIp(string ip, CancellationToken ct = default)
    {
        return await _dbContext.Devices
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Ip == ip, ct);
    }

    public async Task<List<DeviceEntity>> GetAllDevices(CancellationToken ct = default)
    {
        return await _dbContext.Devices
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<List<DeviceEntity>> GetActiveDevices(bool tracked, CancellationToken ct = default)
    {
        var query = _dbContext.Devices.AsQueryable();

        if (!tracked)
            query = query.AsNoTracking();
        
        return await query
            .Where(x => x.IsActive)
            .ToListAsync(ct);
    }

    public async Task AddDevice(DeviceEntity newEntity, CancellationToken ct = default)
    {
        await _dbContext.Devices.AddAsync(newEntity, ct);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task UpdateEntity(Guid id, string name, string ip, int? port, bool isActive, CancellationToken ct = default)
    {
        var entity = await GetDeviceById(id, true, ct);

        entity.Name = name;
        entity.Ip = ip;
        entity.Port = port;
        entity.IsActive = isActive;

        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task DeleteDevice(Guid deviceId, CancellationToken ct = default)
    {
        var entity = await GetDeviceById(deviceId, true, ct);

        _dbContext.Devices.Remove(entity);
        await _dbContext.SaveChangesAsync(ct);
    }
}