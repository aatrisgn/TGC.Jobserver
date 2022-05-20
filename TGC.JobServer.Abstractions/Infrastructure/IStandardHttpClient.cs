namespace TGC.JobServer.Abstractions.Infrastructure
{
    public interface IStandardHttpClient
    {
        HttpClient CreateClient();
    }
}
