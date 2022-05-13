using TGC.JobServer.Abstractions.Jobs;
using TGC.JobServer.Abstractions.Services;
using TGC.JobServer.Models.DTOs;

namespace TGC.JobServer.Jobs.JobExecutionStrategies;

public class DelayedExecutionService : IRecurringService
{
    public bool Accept(string recurringTypeName)
    {
        return recurringTypeName.ToLower() == JobExecutionReferences.DELAYED.ToLower();
    }

    public string Enqueue(JobRequest jobRequest, IInvokeableJob invokeableJob)
    {
        throw new NotImplementedException();
    }
}
