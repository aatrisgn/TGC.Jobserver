using TGC.JobServer.Abstractions.Jobs;
using TGC.JobServer.Models.DTOs;

namespace TGC.JobServer.Abstractions.Services;

public interface IExecutionService
{
    bool Accept(string executionTypeName);
    string Enqueue(JobRequest jobRequest, IInvokeableJob invokeableJob);
}
