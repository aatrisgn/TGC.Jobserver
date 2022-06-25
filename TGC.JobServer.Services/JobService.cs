using Hangfire;
using Hangfire.Storage.Monitoring;
using System.Collections.ObjectModel;
using TGC.JobServer.Abstractions.Infrastructure;
using TGC.JobServer.Abstractions.Jobs;
using TGC.JobServer.Abstractions.Services;
using TGC.JobServer.Models.DTOs;

namespace TGC.JobServer.Services;
public class JobService : IJobService
{
    private IEnumerable<IInvokeableJob> _invokeableJobs;
    private IJobTypeResolver _jobTypeResolver;
    private IJobExecutionTypeResolver _jobExecutionTypeResolver;
    private ICustomMonitoringApi _customMonitoringApi;

    public JobService(IEnumerable<IInvokeableJob> invokeableJobs, IJobTypeResolver jobTypeResolver, IJobExecutionTypeResolver jobExecutionTypeResolver, ICustomMonitoringApi customMonitoringApi)
    {
        _invokeableJobs = invokeableJobs;
        _jobTypeResolver = jobTypeResolver;
        _jobExecutionTypeResolver = jobExecutionTypeResolver;
        _customMonitoringApi = customMonitoringApi;
    }

    private ICollection<IInvokeableJob> GetJobsToInvoke(string jobReference)
    {
        return _invokeableJobs.Where(i => i.Accept(jobReference) == true).ToList();
    }

    public JobDetailsDto GetJobStatusById(string jobId)
    {
        return _customMonitoringApi.JobDetails(jobId);
    }

    public bool JobExists(string jobId)
    {
        return _customMonitoringApi.JobDetails(jobId) == null;
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
        var resolvedExecutionType = _jobExecutionTypeResolver.Resolve(jobRequest);

        var jobId = resolvedExecutionType.Enqueue(jobRequest, resolvedJobType);

        return jobId;
    }

    public IEnumerable<string> GetStartupJobIds()
    {
        return _customMonitoringApi.GetJobsInitializedOnStartup();
    }

    public bool DeleteJob(string jobId)
    {
        return BackgroundJob.Delete(jobId);
    }
}