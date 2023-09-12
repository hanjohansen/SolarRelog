using MediatR;
using Microsoft.EntityFrameworkCore;
using SolarRelog.Application.Exceptions;
using SolarRelog.Application.Logging.Data;

namespace SolarRelog.Application.Queries;

public record GetLogsQuery(DateTime? From, DateTime? To, LogLevel? Level) : IRequest<List<LogEntity>>;

public class GetLogsQueryHandler : IRequestHandler<GetLogsQuery, List<LogEntity>>
{
    private readonly LogDbContext _logDb;

    public GetLogsQueryHandler(LogDbContext logDb)
    {
        _logDb = logDb;
    }

    public async Task<List<LogEntity>> Handle(GetLogsQuery request, CancellationToken cancellationToken)
    {
        if (request.From == null && request.To == null)
            throw new AppException("provide at least one date");

        var fDate = request.From ?? DateTime.Now;
        var tDate = request.To ?? DateTime.Now;
        var both = new List<DateTime>() { fDate, tDate };

        var min = both.Min();
        var max = both.Max();

        var timeSpan = max - min;
        if(timeSpan.TotalDays > 90)
            throw new AppException("requested timespan must not exceed 90 days");
        
        var query = _logDb.Logs
            .AsNoTracking();
            
        query = request.Level != null 
            ? query.Where(x => x.TimeStamp > min && x.TimeStamp < max && x.Level == request.Level) 
            : query.Where(x => x.TimeStamp > min && x.TimeStamp < max);
        
        var logs = await query
            .AsNoTracking()
            .OrderBy(x => x.TimeStamp)
            .ToListAsync(cancellationToken);

        return logs;
    }
}