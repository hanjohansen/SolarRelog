using Microsoft.AspNetCore.Mvc;
using SolarRelog.Application.Commands.Device;
using SolarRelog.Application.Queries;
using SolarRelog.Domain.Entities;

namespace SolarRelog.Controllers;

public class DeviceController : Endpoint
{
    /// <summary>
    /// Get a list of all registered devices
    /// </summary>
    [HttpGet(template:"devices")]
    public async Task<ActionResult<List<DeviceEntity>>> GetDevices()
    {
        var result = await Mediator.Send(new GetDevicesQuery());
        return result;
    }
    
    /// <summary>
    /// Add a new SolarLog Base device
    /// </summary>
    [HttpPost(template:"devices")]
    public async Task<IActionResult> AddDevice(AddDeviceCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }
    
    /// <summary>
    /// Change a registered SolarLog Base device
    /// </summary>
    [HttpPatch(template:"devices")]
    public async Task<IActionResult> UpdateDevice(UpdateDeviceCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }
    
    /// <summary>
    /// Remove a registered SolarLog Base device and all logged data
    /// </summary>
    [HttpDelete(template:"devices")]
    public async Task<IActionResult> DeleteDevice(DeleteDeviceCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }
}