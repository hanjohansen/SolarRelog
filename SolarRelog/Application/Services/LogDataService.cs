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