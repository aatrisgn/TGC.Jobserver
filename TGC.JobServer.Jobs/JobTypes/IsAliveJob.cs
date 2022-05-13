using TGC.JobServer.Abstractions.Jobs;
using TGC.JobServer.Models;
using TGC.JobServer.Models.DTOs;

namespace TGC.JobServer.Jobs;

public class IsAliveJob : IInvokeableJob, IInitializeOnStartup
{
    public bool Accept(string jobReference)
    {
        return jobReference == JobTypeReferences.IS_ALIVE_JOB;
    }

    public void Execute(HangfireJobPayload jobRequest)
    {
        throw new NotImplementedException();
    }

    public void InitializeOnStartup()
    {
        
    }
}
