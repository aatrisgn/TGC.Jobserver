using System.Text.Json;
using System.Text.Json.Nodes;

namespace TGC.JobServer.Models.DTOs
{
    public class JobRequest
    {
        public string JobTypeReference { get; set; }
        public string JobRecurringTypeReference { get; set; }
        public JsonDocument JobTypeInformation { set; get; }
        public JsonDocument JobExecutionTypeInformation { set; get; }

        public string GetJobTypeInformationForHangfireJob()
        {
            return JobTypeInformation.ToString();
        }
    }
}
