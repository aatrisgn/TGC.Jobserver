namespace TGC.JobServer.Models.DTOs
{
    public class JobResponse
    {
        public IEnumerable<string> JobIds;
        public JobResponse(IEnumerable<string> jobids)
        {
            JobIds = jobids;
        }
    }
}
