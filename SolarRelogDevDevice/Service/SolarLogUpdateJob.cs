using Quartz;

namespace SolarRelog.DevDevice.Service
{
    public class SolarLogUpdateJob : IJob
    {
        private readonly SolarLogRecordService _recordService;

        public SolarLogUpdateJob(SolarLogRecordService recordService){
            _recordService = recordService;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _recordService.UpdateData();
            return Task.FromResult(true);
        }
    }
}