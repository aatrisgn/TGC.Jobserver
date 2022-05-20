using TGC.JobServer.Abstractions.Services;
using TGC.JobServer.Models.DTOs;

namespace TGC.JobServer.Infrastructure;

public class JobExecutionTypeResolver : IJobExecutionTypeResolver
{
    private readonly IEnumerable<IExecutionService> _exectionServices;

    public JobExecutionTypeResolver(IEnumerable<IExecutionService> exectionServices)
    {
        _exectionServices = exectionServices;
    }

    public IExecutionService Resolve(JobRequest jobRequest)
    {
        return _exectionServices.First(r => r.Accept(jobRequest.JobExecutionTypeReference));
    }
}
