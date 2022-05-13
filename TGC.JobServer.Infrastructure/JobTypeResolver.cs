using TGC.JobServer.Abstractions.Jobs;
using TGC.JobServer.Abstractions.Services;
using TGC.JobServer.Models.DTOs;

namespace TGC.JobServer.Infrastructure;

public class JobTypeResolver : IJobTypeResolver
{
    private readonly IEnumerable<IInvokeableJob> _invokeables;

    public JobTypeResolver(IEnumerable<IInvokeableJob> invokeables)
    {
        _invokeables = invokeables;
    }

    public IInvokeableJob Resolve(JobRequest jobRequest)
    {
        return _invokeables.First(i => i.Accept(jobRequest.JobTypeReference));
    }
}
