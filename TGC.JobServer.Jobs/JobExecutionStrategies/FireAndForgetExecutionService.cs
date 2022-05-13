using Hangfire;
using System.Text.Json;
using TGC.JobServer.Abstractions.Jobs;
using TGC.JobServer.Abstractions.Services;
using TGC.JobServer.Jobs.JobExecutionStrategies.Containers;
using TGC.JobServer.Models;
using TGC.JobServer.Models.DTOs;

namespace TGC.JobServer.Jobs.JobExecutionStrategies;

public class FireAndForgetExecutionService : IRecurringService
{
    public bool Accept(string recurringTypeName)
    {
        return recurringTypeName.ToLower() == JobExecutionReferences.FIRE_AND_FORGET.ToLower();
    }

    public string Enqueue(JobRequest jobRequest, IInvokeableJob invokeableJob)
    {
        var some = JsonSerializer.Deserialize<FireAndForgetDescriber>(jobRequest.JobExecutionTypeInformation);
        //TODO: Consider moving to base class
        var jobId = BackgroundJob.Enqueue(() => invokeableJob.Execute(new HangfireJobPayload(jobRequest)));
        return jobId;
    }
}
