using Microsoft.EntityFrameworkCore;
using SolarRelog.Application.ServiceInterfaces;
using SolarRelog.Domain.Entities;
using SolarRelog.Infrastructure;

namespace SolarRelog.Application.Services;

public class LogDataService : ILogDataService
{
    private readonly AppDbContext _dbContext;

    public LogDataService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<LogDataEntity>> GetLogData(Guid deviceId, int year, int month, CancellationToken ct = default)
    {
        var first = new DateTime(year, month, 1);
        var last = new DateTime(year, month + 1, 1).AddSeconds(-1);

        var result = await _dbContext.Logs
            .AsNoTracking()
            .Include(x => x.ConsumerData)
            .Where(x => x.RecordDate > first && x.RecordDate < last && x.DeviceId == deviceId)
            .OrderBy(x => x.RecordDate)
            .ToListAsync(ct);

        return result;
    }

    public async Task AddLogData(List<LogDataEntity> entities, CancellationToken ct = default)
    {
        await _dbContext.Logs.AddRangeAsync(entities, ct);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task DeleteLogDataAfter(DateTime refDate, CancellationToken ct = default)
    {
        await _dbContext.Logs
            .Where(x => x.LoggedDate < refDate)
            .ExecuteDeleteAsync(ct);
    }
}