namespace TGC.JobServer.Abstractions.Infrastructure
{
    public interface IStandardHttpClient
    {
        HttpClient CreateClient();

        StringContent GenerateStringContent(string requestBody);
    }
}
