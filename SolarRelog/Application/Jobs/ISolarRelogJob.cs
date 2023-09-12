using Quartz;

namespace SolarRelog.Application.Jobs;

public interface ISolarRelogJob : IJob
{
    static JobKey JobKey { get; }
}