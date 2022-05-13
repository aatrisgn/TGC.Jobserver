using TGC.JobServer.Abstractions.Jobs;
using TGC.JobServer.Abstractions.Services;
using TGC.JobServer.Models.DTOs;

namespace TGC.JobServer.Jobs.JobExecutionStrategies;

public class BatchExecutionService : IRecurringService
{
    public bool Accept(string recurringTypeName)
    {
        return recurringTypeName.ToLower() == JobExecutionReferences.BATCH.ToLower();
    }

    public string Enqueue(JobRequest jobRequest, IInvokeableJob invokeableJob)
    {
        throw new NotImplementedException();
    }
}
