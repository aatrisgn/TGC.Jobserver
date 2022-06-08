using System.Text.Json;
using TGC.JobServer.Models.DTOs;

namespace TGC.JobServer.Models;

public class HangfireJobPayload
{
    public HangfireJobPayload(JobRequest jobRequest)
    {
        this.JobTypeInformation = JsonSerializer.Serialize(jobRequest.JobTypeInformation);
        this.JobCallback = jobRequest.JobCallback;
    }

    public HangfireJobPayload()
    {

    }

    public string JobTypeInformation { set; get; }
    public JobCallback JobCallback { set; get; }
}
