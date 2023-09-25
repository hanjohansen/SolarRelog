using Microsoft.AspNetCore.Mvc;
using SolarRelog.Application.Queries;
using SolarRelog.Domain.Entities;

namespace SolarRelog.Controllers;

public class LogRecordsController : Endpoint
{
    /// <summary>
    /// Query device logs - JSON formatted
    /// </summary>
    [HttpGet(template:"records/json")]
    public async Task<ActionResult<List<LogDataEntity>>> GetJson([FromQuery]Guid deviceId, [FromQuery]int year,  [FromQuery]int month)
    {
        var result = await Mediator.Send(new GetLogRecordsJsonQuery(deviceId, year, month));
        return result;
    }
    
    /// <summary>
    /// Query device logs - CSV formatted
    /// </summary>
    [HttpGet(template:"records/csv")]
    public async Task<IActionResult> GetCsv([FromQuery]Guid deviceId, [FromQuery]int year,  [FromQuery]int month)
    {
        var result = await Mediator.Send(new GetLogRecordsCsvQuery(deviceId, year, month));
        result.Content.Position = 0;
        
        return File(result.Content, result.ContentType, result.FileName);
    }
}
