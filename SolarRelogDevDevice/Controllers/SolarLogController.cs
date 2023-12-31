using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SolarRelog.DevDevice.Domain;
using SolarRelog.DevDevice.Service;

namespace SolarRelog.DevDevice.Controllers;

[ApiController]
public class SolarLogController : ControllerBase
{
    private readonly SolarLogRecordService _recordService;

    public SolarLogController(SolarLogRecordService recordService){
        _recordService = recordService;
    }

    [HttpPost]
    [Route("getjp", Name= "GetSolarData")]
    public ActionResult<SolarLogRequest> GetData([FromBody] object content)
    {
        JsonElement jsonElement;

        try
        {
            jsonElement = JsonSerializer.Deserialize<JsonElement>(content.ToString() ?? string.Empty);
        }
        catch
        {
            return BadRequest("invalid request content. valid values: {\"801\":{\"170\":null}, \"782\":null}");
        }

        var includeRecord = false;
        var includeConsumers = false;
        
        foreach (var property in jsonElement.EnumerateObject().OfType<JsonProperty>())
        {
            if (property.Name.Equals("801", StringComparison.OrdinalIgnoreCase))
                includeRecord = true;
            if (property.Name.Equals("782", StringComparison.OrdinalIgnoreCase))
                includeConsumers = true;
        }

        if (!includeRecord && !includeConsumers)
            return BadRequest("invalid request content. valid values: {\"801\":{\"170\":null}, \"782\":null}");

        var record = _recordService.GetLogRecord(includeRecord, includeConsumers);
        
        return Ok(record);
    }
}
