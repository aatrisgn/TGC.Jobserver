namespace TGC.JobServer.Abstractions.Services;
public interface IJobCallbackService
{
    void SendPostRequestToCallbackUrl(string url, object requestBody);
}
