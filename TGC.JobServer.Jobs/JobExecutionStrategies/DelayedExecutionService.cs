using Hangfire;
using TGC.JobServer.Abstractions.Infrastructure;
using TGC.JobServer.Abstractions.Jobs;
using TGC.JobServer.Abstractions.Services;
using TGC.JobServer.Jobs.JobExecutionStrategies.Containers;
using TGC.JobServer.Models;
using TGC.JobServer.Models.DTOs;

namespace TGC.JobServer.Jobs.JobExecutionStrategies;

public class DelayedExecutionService : IExecutionService
{
    private readonly IJsonSerializer _jsonSerializer;
    public DelayedExecutionService(IJsonSerializer jsonSerializer)
    {
        _jsonSerializer = jsonSerializer;
    }

    public bool Accept(string executionTypeName)
    {
        return executionTypeName.ToLower() == JobExecutionReferences.DELAYED.ToLower();
    }

    public string Enqueue(JobRequest jobRequest, IInvokeableJob invokeableJob)
    {
        var delayedExecutionDescriber = _jsonSerializer.Deserialize<DelayedExecutionDescriber>(jobRequest.JobExecutionTypeInformation);
        //TODO: Consider creating an abstraction for initializng a HanfireJobPayload, since we are currently dependent on an implementation inside HangfireJobPayload, which can cause inconsistency.

        //null being parsed as parameter since PerformContext is automatically set by Hangfire
        var jobId = BackgroundJob.Schedule(
            () => invokeableJob.Execute(new HangfireJobPayload(jobRequest), null),
            DetermineDelay(delayedExecutionDescriber));

        return jobId;
    }

    private TimeSpan DetermineDelay(DelayedExecutionDescriber delayedExecutionDescriber)
    {
        switch (delayedExecutionDescriber.DelayedType)
        {
            case (1):
                return TimeSpan.FromDays(delayedExecutionDescriber.DelayedAmount);
            case (2):
                return TimeSpan.FromHours(delayedExecutionDescriber.DelayedAmount);
            case (3):
                return TimeSpan.FromMinutes(delayedExecutionDescriber.DelayedAmount);
            case (4):
                return TimeSpan.FromSeconds(delayedExecutionDescriber.DelayedAmount);
            case(5):
                return TimeSpan.FromMilliseconds(delayedExecutionDescriber.DelayedAmount);
            default:
                throw new ArgumentException("System was not able to determine type of delay");
        }
    }
}
