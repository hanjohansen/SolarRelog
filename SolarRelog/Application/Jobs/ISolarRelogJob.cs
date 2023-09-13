using Quartz;

namespace SolarRelog.Application.Jobs;

public interface ISolarRelogJob : IJob
{
    static JobKey Key { get; }
}