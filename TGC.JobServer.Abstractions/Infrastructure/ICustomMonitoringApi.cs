using Hangfire.Storage;

namespace TGC.JobServer.Abstractions.Infrastructure;
public interface ICustomMonitoringApi : IMonitoringApi
{
    IEnumerable<string> GetJobsInitializedOnStartup();
    void AddJobIdInitializedOnStartup(string jobId);
}
