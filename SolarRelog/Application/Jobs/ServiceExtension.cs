using Quartz;
using SolarRelog.Application.Jobs.LogRecords;
using SolarRelog.Application.Jobs.Logs;

namespace SolarRelog.Application.Jobs;

public static class ServiceExtension
{
    public static WebApplicationBuilder ConfigureAppJobs(this WebApplicationBuilder builder)
    {
        builder.Services.AddQuartz(q =>
        {
            q.AddJob<LogCleanupJob>(opts => opts
                .WithIdentity(LogCleanupJob.Key)
                .StoreDurably());
            q.AddTrigger(opts => opts
                .ForJob(LogCleanupJob.Key)
                .StartAt(DateTimeOffset.UtcNow.AddSeconds(15)));

            
            
            q.AddJob<LogDataJob>(opts => opts
                .WithIdentity(LogDataJob.Key)
                .StoreDurably());
            q.AddTrigger(opts => opts
                .ForJob(LogDataJob.Key)
                .StartAt(DateTimeOffset.UtcNow.AddSeconds(15)));
            
            q.AddJob<LogDataCleanUpJob>(opts => opts
                .WithIdentity(LogDataCleanUpJob.Key)
                .StoreDurably());
            q.AddTrigger(opts => opts
                .ForJob(LogDataCleanUpJob.Key)
                .StartAt(DateTimeOffset.UtcNow.AddSeconds(15)));
        });
        
        builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        
        return builder;
    }
}