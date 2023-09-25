using MediatR;
using SolarRelog.Application.ServiceInterfaces;
using SolarRelog.Domain.Entities;

namespace SolarRelog.Application.Queries;

public record GetLogRecordsJsonQuery(Guid DeviceId, int Year, int Month) : IRequest<List<LogDataEntity>>;

public class GetLogRecordsJsonQueryHandler : IRequestHandler<GetLogRecordsJsonQuery, List<LogDataEntity>>
{
    private readonly ILogDataService _logData;

    public GetLogRecordsJsonQueryHandler(ILogDataService logData)
    {
        _logData = logData;
    }

    public async Task<List<LogDataEntity>> Handle(GetLogRecordsJsonQuery request, CancellationToken cancellationToken)
    {
        return await _logData.GetLogData(request.DeviceId, request.Year, request.Month, cancellationToken);
    }
}