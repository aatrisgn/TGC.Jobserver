
using Hangfire.Storage.Monitoring;
using TGC.JobServer.Models.DTOs;

namespace TGC.JobServer.Abstractions.Services;
public interface IJobService
{
    ICollection<string> HandleJobs(IEnumerable<JobRequest> jobRequests);
    string HandleJob(JobRequest jobRequests);
    JobDetailsDto GetJobStatusById(string jobId);

    bool JobExists(string jobId);
    IEnumerable<string> GetStartupJobIds();
    bool DeleteJob(string jobId); 
}
