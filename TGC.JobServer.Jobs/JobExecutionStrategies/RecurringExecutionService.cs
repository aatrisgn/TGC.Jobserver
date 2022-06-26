using Hangfire;
using TGC.JobServer.Abstractions.Infrastructure;
using TGC.JobServer.Abstractions.Jobs;
using TGC.JobServer.Abstractions.Services;
using TGC.JobServer.Jobs.JobExecutionStrategies.Containers;
using TGC.JobServer.Models;
using TGC.JobServer.Models.DTOs;

namespace TGC.JobServer.Jobs.JobExecutionStrategies;

public class RecurringExecutionService : IExecutionService
{
    private readonly IJsonSerializer _jsonSerializer;
    private readonly ICustomMonitoringApi _customMonitoringApi;
    private readonly IJobEngine _jobEngine;

    public RecurringExecutionService(IJsonSerializer jsonSerializer, ICustomMonitoringApi customMonitoringApi, IJobEngine jobEngine)
    {
        _jsonSerializer = jsonSerializer;
        _customMonitoringApi = customMonitoringApi;
        _jobEngine = jobEngine;
    }

    public bool Accept(string executionTypeName)
    {
        return executionTypeName.ToLower() == JobExecutionReferences.RECURRING.ToLower();
    }

    public string Enqueue(JobRequest jobRequest, IInvokeableJob invokeableJob)
    {
        var recurringExecutionDescriber = _jsonSerializer.Deserialize<RecurringExecutionDescriber>(jobRequest.JobExecutionTypeInformation);
        //TODO: Consider creating an abstraction for initializng a HanfireJobPayload, since we are currently dependent on an implementation inside HangfireJobPayload, which can cause inconsistency.
        var jobWithIdExists = _customMonitoringApi.RecurringJobExists(recurringExecutionDescriber.RecurringJobName);
        var jobShouldUpdate = recurringExecutionDescriber.ShouldUpdate;

        if (jobWithIdExists)
        {
            if (jobShouldUpdate)
            {
                return AddOrUpdateJob(recurringExecutionDescriber, invokeableJob, jobRequest);
            } 
            else
            {
                throw new ArgumentException("Job with same ID already exists");
            }
        } 
        else if (jobWithIdExists == false && jobShouldUpdate)
        {
            throw new ArgumentException("No job with specified ID exists");
        }
        else
        {
            return AddOrUpdateJob(recurringExecutionDescriber, invokeableJob, jobRequest);
        }
    }

    public string AddOrUpdateJob(RecurringExecutionDescriber describer, IInvokeableJob job, JobRequest  jobRequest)
    {
        _jobEngine.Recurring(
                describer.RecurringJobName,
                () => job.Execute(new HangfireJobPayload(jobRequest), null),
                describer.CronExpression
            );

        return describer.RecurringJobName;
    }
}
