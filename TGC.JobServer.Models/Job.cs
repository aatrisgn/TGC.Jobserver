using TGC.JobServer.Models.Enums;

namespace TGC.JobServer.Models;
public class Job
{
    public Guid Id { get; set; }
    public string JobReference { get; set; }
    public JobExecutionTime jobExecutionTime { get; set; }

}
