namespace TGC.JobServer.Jobs.JobTypes.Containers
{
    public class HttpJobDescriber
    {
        public string HttpMethod { get; set; }
        public string Url { get; set; }
        public string RequestPayload { get; set; }
    }
}
