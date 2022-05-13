using TGC.JobServer.Abstractions.Jobs;
using TGC.JobServer.Abstractions.Services;
using TGC.JobServer.Models.DTOs;

namespace TGC.JobServer.Infrastructure;

public class JobRecurringTypeResolver : IJobRecurringTypeResolver
{
    private readonly IEnumerable<IRecurringService> _recurringServices;

    public JobRecurringTypeResolver(IEnumerable<IRecurringService> recurringServices)
    {
        _recurringServices = recurringServices;
    }

    public IRecurringService Resolve(JobRequest jobRequest)
    {
        return _recurringServices.First(r => r.Accept(jobRequest.JobRecurringTypeReference));
    }
}
