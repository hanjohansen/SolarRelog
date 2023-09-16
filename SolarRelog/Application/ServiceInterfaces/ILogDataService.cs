using SolarRelog.Domain.Entities;

namespace SolarRelog.Application.ServiceInterfaces;

public interface ILogDataService
{
    Task AddLogData(List<LogDataEntity> entities, CancellationToken ct = default);
}