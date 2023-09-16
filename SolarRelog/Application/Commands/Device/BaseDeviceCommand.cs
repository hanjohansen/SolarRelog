using Microsoft.EntityFrameworkCore;
using Quartz;
using SolarRelog.Application.Jobs.LogRecords;
using SolarRelog.Infrastructure;

namespace SolarRelog.Application.Commands.Device;

public class BaseDeviceCommandHandler
{
    protected async Task UnpausePollingJob(AppDbContext context, ISchedulerFactory schedulerFactory, ILogger logger)
    {
        var devices = await context.Devices
            .AsNoTracking()
            .Where(x => x.IsActive)
            .ToListAsync();

        if (!devices.Any())
            return;
        
        logger.LogInformation("Resuming to poll data");
        
        var scheduler = await schedulerFactory.GetScheduler();
        await scheduler.ResumeJob(LogDataJob.Key);
    }
}