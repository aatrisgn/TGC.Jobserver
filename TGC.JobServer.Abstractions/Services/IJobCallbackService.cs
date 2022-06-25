using TGC.JobServer.Models;

namespace TGC.JobServer.Abstractions.Services;
public interface IJobCallbackService
{
    void SendPostRequestToCallbackUrl(string url, ICallbackResponse requestBody);
}
