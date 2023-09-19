using Quartz;
using SolarRelog.Application.Jobs.LogRecords;
using SolarRelog.Application.ServiceInterfaces;

namespace SolarRelog.Application.Commands.Device;

public class BaseDeviceCommandHandler
{
    protected async Task UnpausePollingJob(IDeviceService deviceService, ISchedulerFactory schedulerFactory, ILogger logger)
    {
        var devices = await deviceService.GetActiveDevices(false);

        if (!devices.Any())
            return;
        
        logger.LogInformation("Resuming to poll data");
        
        var scheduler = await schedulerFactory.GetScheduler();
        await scheduler.ResumeJob(LogDataJob.Key);
    }
}