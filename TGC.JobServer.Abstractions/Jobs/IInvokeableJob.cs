using Hangfire.Server;
using TGC.JobServer.Models;

namespace TGC.JobServer.Abstractions.Jobs;

public interface IInvokeableJob
{
    bool Accept(string jobReference);
    void Execute(HangfireJobPayload jobRequest, PerformContext context);
}
