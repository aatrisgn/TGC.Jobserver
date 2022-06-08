using TGC.JobServer.Abstractions.Infrastructure;
using TGC.JobServer.Abstractions.Services;

namespace TGC.JobServer.Services;

public class JobCallbackService : IJobCallbackService
{
    private readonly IStandardHttpClient _standardHttpClient;
    private readonly IJsonSerializer _jsonSerializer;
    public JobCallbackService(IStandardHttpClient standardHttpClient, IJsonSerializer jsonSerializer)
    {
        _standardHttpClient = standardHttpClient;
        _jsonSerializer = jsonSerializer;
    }

    public void SendPostRequestToCallbackUrl(string url, object requestBody)
    {
        using(var httpClient = _standardHttpClient.CreateClient())
        {
            var serializedRequestBody = _jsonSerializer.Serialize(requestBody);
            var requestContentBody = _standardHttpClient.GenerateStringContent(serializedRequestBody);
            var response = httpClient.PostAsync(url, requestContentBody).Result;
            response.EnsureSuccessStatusCode();
        }
    }
}
