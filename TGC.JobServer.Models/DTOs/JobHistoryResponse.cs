using Hangfire.Storage.Monitoring;

namespace TGC.JobServer.Models.DTOs;

public class JobHistoryResponse
{
    public int JobId { get; set; }
    public IList<StateHistoryDto> History { get; set; }
    
    public JobHistoryResponse(JobDetailsDto jobStatusDto, int jobId)
    {
        this.History = jobStatusDto.History;
        this.JobId = jobId;
    }
}
