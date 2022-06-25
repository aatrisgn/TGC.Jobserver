using Hangfire.Server;
using TGC.JobServer.Abstractions.Jobs;
using TGC.JobServer.Models;

namespace TGC.JobServer.Jobs;

public class IsAliveJob : IInvokeableJob
{
    public bool Accept(string jobReference)
    {
        return jobReference == JobTypeReferences.IS_ALIVE_JOB;
    }

    public void Execute(HangfireJobPayload jobRequest, PerformContext context)
    {
        throw new NotImplementedException();
    }
}
