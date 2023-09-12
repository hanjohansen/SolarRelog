using Microsoft.AspNetCore.Mvc;
using SolarRelog.Application.Logging.Data;
using SolarRelog.Application.Queries;

namespace SolarRelog.Controllers;

public class LogsController : Endpoint
{
    [HttpGet(template:"logs")]
    public async Task<ActionResult<List<LogEntity>>> Get([FromQuery]DateTime? from,  [FromQuery]DateTime? to, [FromQuery]LogLevel? level)
    {
        var result = await Mediator.Send(new GetLogsQuery(from, to, level));
        return result;
    }
}
