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
    public ActionResult<SolarLogRequest> GetData()
    {
        var record = _recordService.GetLogRecord();
        return Ok(record);
    }
}
