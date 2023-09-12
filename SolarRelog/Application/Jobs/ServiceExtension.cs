using Quartz;
using SolarRelog.Application.Jobs.Logs;

namespace SolarRelog.Application.Jobs;

public static class ServiceExtension
{
    public static WebApplicationBuilder ConfigureAppJobs(this WebApplicationBuilder builder)
    {
        builder.Services.AddQuartz(q =>
        {
            var jobKey = LogCleanupJob.JobKey;
            q.AddJob<LogCleanupJob>(opts => 
                opts.WithIdentity(jobKey)
                    .StoreDurably());
            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .StartAt(DateTimeOffset.UtcNow.AddSeconds(30)));
        });
        
        builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        
        return builder;
    }
}