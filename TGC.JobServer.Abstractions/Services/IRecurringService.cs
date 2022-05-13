using TGC.JobServer.Abstractions.Jobs;
using TGC.JobServer.Models.DTOs;

namespace TGC.JobServer.Abstractions.Services;

public interface IRecurringService
{
    bool Accept(string recurringTypeName);
    string Enqueue(JobRequest jobRequest, IInvokeableJob invokeableJob);
}
