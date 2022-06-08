using System.Text;
using TGC.JobServer.Abstractions.Infrastructure;

namespace TGC.JobServer.Infrastructure
{
    public class StandardHttpClient : IStandardHttpClient
    {
        public HttpClient CreateClient()
        {
            return new HttpClient();
        }

        public HttpClient CreateClient(string baseUrl)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseUrl);
            return httpClient;
        }

        public StringContent GenerateStringContent(string requestBody)
        {
            return new StringContent(requestBody, Encoding.UTF8, "application/json");
        }
    }
}
