using TGC.JobServer.Abstractions.Infrastructure;

namespace TGC.JobServer.Infrastructure
{
    public class StandardHttpClient : IStandardHttpClient
    {
        public HttpClient CreateClient()
        {
            return new HttpClient();
        }
    }
}
