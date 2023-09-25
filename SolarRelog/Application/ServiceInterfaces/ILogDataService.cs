using SolarRelog.Domain.Entities;

namespace SolarRelog.Application.ServiceInterfaces;

public interface ILogDataService
{
    Task<List<LogDataEntity>> GetLogData(Guid deviceId, int year, int month, CancellationToken ct = default);
    
    Task AddLogData(List<LogDataEntity> entities, CancellationToken ct = default);

    Task DeleteLogDataAfter(DateTime after, CancellationToken ct = default);
}