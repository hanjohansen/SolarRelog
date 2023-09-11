using Microsoft.AspNetCore.Mvc;
using SolarRelog.Application.Commands.Settings;
using SolarRelog.Application.Queries;
using SolarRelog.Domain.Dto;

namespace SolarRelog.Controllers.Settings;

public class SettingsController : Endpoint
{
    [HttpGet(template:"settings")]
    public async Task<ActionResult<SettingsModel>> Get()
    {
        var result = await Mediator.Send(new GetSettingsQuery());
        return result;
    }
    
    [HttpPost(template:"settings")]
    public async Task<IActionResult> Update(SettingsModel model)
    {
        await Mediator.Send(new UpdateSettingsCommand(model));
        return Ok();
    }
}