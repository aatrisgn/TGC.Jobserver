using TGC.JobServer.Abstractions.Infrastructure;
using TGC.JobServer.Abstractions.Jobs;
using TGC.JobServer.Abstractions.Services;
using TGC.JobServer.Jobs.JobExecutionStrategies.Containers;
using TGC.JobServer.Models;
using TGC.JobServer.Models.DTOs;

namespace TGC.JobServer.Jobs.JobExecutionStrategies;

public class FireAndForgetExecutionService : IExecutionService
{
    private readonly IJsonSerializer _jsonSerializer;
    private readonly IJobEngine _jobEngine;
    public FireAndForgetExecutionService(IJsonSerializer jsonSerializer, IJobEngine jobEngine)
    {
        _jsonSerializer = jsonSerializer;
        _jobEngine = jobEngine;
    }

    public bool Accept(string executionTypeName)
    {
        return executionTypeName.ToLower() == JobExecutionReferences.FIRE_AND_FORGET.ToLower();
    }

    public string Enqueue(JobRequest jobRequest, IInvokeableJob invokeableJob)
    {
        var fireAndForgetDescriber = _jsonSerializer.Deserialize<FireAndForgetDescriber>(jobRequest.JobExecutionTypeInformation);
        //TODO: Consider creating an abstraction for initializng a HanfireJobPayload, since we are currently dependent on an implementation inside HangfireJobPayload, which can cause inconsistency.

        //null being parsed as parameter since PerformContext is automatically set by Hangfire
        var jobId = _jobEngine.FireAndForget(() => invokeableJob.Execute(new HangfireJobPayload(jobRequest), null));
        return jobId;
    }
}
