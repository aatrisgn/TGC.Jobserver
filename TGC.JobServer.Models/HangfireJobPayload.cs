using System.Text.Json;
using TGC.JobServer.Models.DTOs;

namespace TGC.JobServer.Models;

public class HangfireJobPayload
{
    public HangfireJobPayload(JobRequest jobRequest)
    {
        this.JobTypeInformation = JsonSerializer.Serialize(jobRequest.JobTypeInformation);
    }

    public HangfireJobPayload()
    {

    }

    public string JobTypeInformation { set; get; }
}
