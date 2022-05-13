using Hangfire.Storage;
using Hangfire.Storage.Monitoring;
using System.Collections.ObjectModel;
using TGC.JobServer.Abstractions.Jobs;
using TGC.JobServer.Abstractions.Services;
using TGC.JobServer.Models.DTOs;

namespace TGC.JobServer.Services;
public class JobService : IJobService
{
    private IEnumerable<IInvokeableJob> _invokeableJobs;
    private IJobTypeResolver _jobTypeResolver;
    private IJobRecurringTypeResolver _jobRecurringTypeResolver;
    private IMonitoringApi _monitoringApi;

    public JobService(IEnumerable<IInvokeableJob> invokeableJobs, IJobTypeResolver jobTypeResolver, IJobRecurringTypeResolver jobRecurringTypeResolver, IMonitoringApi monitoringApi)
    {
        _invokeableJobs = invokeableJobs;
        _jobTypeResolver = jobTypeResolver;
        _jobRecurringTypeResolver = jobRecurringTypeResolver;
        _monitoringApi = monitoringApi;
    }

    private ICollection<IInvokeableJob> GetJobsToInvoke(string jobReference)
    {
        return _invokeableJobs.Where(i => i.Accept(jobReference) == true).ToList();
    }

    public JobDetailsDto GetJobStatusById(int jobId)
    {
        return _monitoringApi.JobDetails(jobId.ToString());
    }

    public ICollection<string> HandleJobs(IEnumerable<JobRequest> jobRequests)
    {
        var jobIds = new Collection<string>();

        foreach (var jobRequest in jobRequests)
        {
            jobIds.Add(HandleJob(jobRequest));

        }

        return jobIds;
    }

    public string HandleJob(JobRequest jobRequest)
    {
        var resolvedJobType = _jobTypeResolver.Resolve(jobRequest);
        var resolvedRecurringType = _jobRecurringTypeResolver.Resolve(jobRequest);

        var jobId = resolvedRecurringType.Enqueue(jobRequest, resolvedJobType);

        return jobId;
    }
}