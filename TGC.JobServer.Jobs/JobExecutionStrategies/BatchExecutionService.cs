using TGC.JobServer.Abstractions.Jobs;
using TGC.JobServer.Abstractions.Services;
using TGC.JobServer.Models.DTOs;

namespace TGC.JobServer.Jobs.JobExecutionStrategies;

public class BatchExecutionService : IExecutionService
{
    public bool Accept(string executionTypeName)
    {
        return executionTypeName.ToLower() == JobExecutionReferences.BATCH.ToLower();
    }

    public string Enqueue(JobRequest jobRequest, IInvokeableJob invokeableJob)
    {
        throw new NotImplementedException();
    }
}
