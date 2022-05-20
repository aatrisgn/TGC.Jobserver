using Hangfire;
using Hangfire.Storage.Monitoring;
using TGC.JobServer.Abstractions.Infrastructure;

namespace TGC.JobServer.Infrastructure
{
    public class MonitoringApi : ICustomMonitoringApi
    {
        private List<string> startupJobIds;
        private object listlock = new object();

        public MonitoringApi()
        {
            startupJobIds = new List<string>();
        }
        public void AddJobIdInitializedOnStartup(string jobId)
        {
            lock (listlock)
            {
                startupJobIds.Add(jobId);
            }
        }

        public JobList<DeletedJobDto> DeletedJobs(int from, int count)
        {
            return JobStorage.Current.GetMonitoringApi().DeletedJobs(from, count);
        }

        public long DeletedListCount()
        {
            return JobStorage.Current.GetMonitoringApi().DeletedListCount();
        }

        public long EnqueuedCount(string queue)
        {
            return JobStorage.Current.GetMonitoringApi().EnqueuedCount(queue);
        }

        public JobList<EnqueuedJobDto> EnqueuedJobs(string queue, int from, int perPage)
        {
            return JobStorage.Current.GetMonitoringApi().EnqueuedJobs(queue, from, perPage);
        }

        public IDictionary<DateTime, long> FailedByDatesCount()
        {
            return JobStorage.Current.GetMonitoringApi().FailedByDatesCount();
        }

        public long FailedCount()
        {
            return JobStorage.Current.GetMonitoringApi().FailedCount();
        }

        public JobList<FailedJobDto> FailedJobs(int from, int count)
        {
            return JobStorage.Current.GetMonitoringApi().FailedJobs(from, count);
        }

        public long FetchedCount(string queue)
        {
            return JobStorage.Current.GetMonitoringApi().FetchedCount(queue);
        }

        public JobList<FetchedJobDto> FetchedJobs(string queue, int from, int perPage)
        {
            return JobStorage.Current.GetMonitoringApi().FetchedJobs(queue, from, perPage);
        }

        public IEnumerable<string> GetJobsInitializedOnStartup()
        {
            return startupJobIds;
        }

        public StatisticsDto GetStatistics()
        {
            return JobStorage.Current.GetMonitoringApi().GetStatistics();
        }

        public IDictionary<DateTime, long> HourlyFailedJobs()
        {
            return JobStorage.Current.GetMonitoringApi().HourlyFailedJobs();
        }

        public IDictionary<DateTime, long> HourlySucceededJobs()
        {
            return JobStorage.Current.GetMonitoringApi().HourlySucceededJobs();
        }

        public JobDetailsDto JobDetails(string jobId)
        {
            return JobStorage.Current.GetMonitoringApi().JobDetails(jobId);
        }

        public long ProcessingCount()
        {
            return JobStorage.Current.GetMonitoringApi().ProcessingCount();
        }

        public JobList<ProcessingJobDto> ProcessingJobs(int from, int count)
        {
            return JobStorage.Current.GetMonitoringApi().ProcessingJobs(from, count);
        }

        public IList<QueueWithTopEnqueuedJobsDto> Queues()
        {
            return JobStorage.Current.GetMonitoringApi().Queues();
        }

        public long ScheduledCount()
        {
            return JobStorage.Current.GetMonitoringApi().ScheduledCount();
        }

        public JobList<ScheduledJobDto> ScheduledJobs(int from, int count)
        {
            return JobStorage.Current.GetMonitoringApi().ScheduledJobs(from, count);
        }

        public IList<ServerDto> Servers()
        {
            return JobStorage.Current.GetMonitoringApi().Servers();
        }

        public IDictionary<DateTime, long> SucceededByDatesCount()
        {
            return JobStorage.Current.GetMonitoringApi().SucceededByDatesCount();
        }

        public JobList<SucceededJobDto> SucceededJobs(int from, int count)
        {
            return JobStorage.Current.GetMonitoringApi().SucceededJobs(from, count);
        }

        public long SucceededListCount()
        {
            return JobStorage.Current.GetMonitoringApi().SucceededListCount();
        }
    }
}