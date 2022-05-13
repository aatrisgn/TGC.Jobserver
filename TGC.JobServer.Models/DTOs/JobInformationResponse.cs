using Hangfire.Storage.Monitoring;

namespace TGC.JobServer.Models.DTOs;

public class JobInformationResponse
{
    public int JobId { get; set; }
    public string State { get; set; }
    public int Retries { get; set; }
    public DateTime? CreatedAt { get; set;}
    public DateTime? ExpireAt { get; set; }
 
    public JobInformationResponse(JobDetailsDto jobStatusDto, int jobId)
    {
        this.JobId = jobId;
        this.ExpireAt = jobStatusDto.ExpireAt;
        this.CreatedAt = jobStatusDto.CreatedAt;
        this.Retries = Int32.Parse(jobStatusDto.Properties.First(p => p.Key == "RetryCount").Value);
        this.State = jobStatusDto.History.OrderByDescending(h => h.CreatedAt).First().StateName;
    }
}