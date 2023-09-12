using Microsoft.AspNetCore.Mvc;
using SolarRelog.Application.Commands.Device;
using SolarRelog.Application.Queries;
using SolarRelog.Domain.Entities;

namespace SolarRelog.Controllers;

public class DeviceController : Endpoint
{
    [HttpGet(template:"devices")]
    public async Task<ActionResult<List<DeviceEntity>>> GetDevices()
    {
        var result = await Mediator.Send(new GetDevicesQuery());
        return result;
    }
    
    [HttpPost(template:"devices")]
    public async Task<IActionResult> AddDevice(AddDeviceCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }
    
    [HttpPatch(template:"devices")]
    public async Task<IActionResult> UpdateDevice(UpdateDeviceCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }
    
    [HttpDelete(template:"devices")]
    public async Task<IActionResult> DeleteDevice(DeleteDeviceCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }
}