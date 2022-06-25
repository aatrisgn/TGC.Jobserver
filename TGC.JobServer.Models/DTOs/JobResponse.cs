namespace TGC.JobServer.Models.DTOs
{
    public class JobResponse
    {
        public List<string> JobIds;
        public JobResponse(IEnumerable<string> jobids)
        {
            JobIds = jobids.ToList();
        }
    }
}
