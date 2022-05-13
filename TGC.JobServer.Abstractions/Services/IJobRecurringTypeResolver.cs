using TGC.JobServer.Abstractions.Jobs;
using TGC.JobServer.Models.DTOs;

namespace TGC.JobServer.Abstractions.Services;
public interface IJobRecurringTypeResolver
{
    IRecurringService Resolve(JobRequest jobRequest);
}
