using Microsoft.Extensions.Logging;
using System.Text.Json;
using TGC.JobServer.Abstractions.Infrastructure;
using TGC.JobServer.Abstractions.Jobs;
using TGC.JobServer.Jobs.JobTypes.Containers;
using TGC.JobServer.Models;

namespace TGC.JobServer.Jobs.JobTypes;

public class HttpJob : IInvokeableJob
{
    private readonly ILogger<HttpJob> _logger;
    private readonly IStandardHttpClient _standardHttpClient;
    private readonly IJsonSerializer _jsonSerializer;

    public HttpJob(ILogger<HttpJob> logger, IStandardHttpClient standardHttpClient, IJsonSerializer jsonSerializer)
    {
        _logger = logger;
        _standardHttpClient = standardHttpClient;
        _jsonSerializer = jsonSerializer;
    }

    public bool Accept(string jobReference)
    {
        return jobReference.ToLower() == JobTypeReferences.HTTP_JOB.ToLower();
    }

    public void Execute(HangfireJobPayload hangfireJobPayload)
    {
        try
        {
            var httpDescriber = _jsonSerializer.Deserialize<HttpJobDescriber>(hangfireJobPayload.JobTypeInformation);

            using (var httpClient = _standardHttpClient.CreateClient())
            {
                _logger.LogInformation($"REQUEST - {httpDescriber.HttpMethod}: {httpDescriber.Url}");

                var response = ExecuteHttpRequest(httpClient, httpDescriber);

                var responseBody = response.Content.ReadAsStringAsync().Result;

                _logger.LogInformation($"RESPONSE - {httpDescriber.HttpMethod}:{httpDescriber.Url} ({response.StatusCode}) - {responseBody}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            throw;
        }
    }

    private HttpResponseMessage ExecuteHttpRequest(HttpClient httpClient, HttpJobDescriber httpJobDescriber)
    {
        switch (httpJobDescriber.HttpMethod)
        {
            case "GET":
                return Get(httpClient, httpJobDescriber);
            case "POST":
                return Post(httpClient, httpJobDescriber);
            case "PUT":
                return Put(httpClient, httpJobDescriber);
            case "DELETE":
                return Delete(httpClient, httpJobDescriber);
            default:
                throw new NotImplementedException();
        }
    }

    private HttpResponseMessage Get(HttpClient httpClient, HttpJobDescriber httpJobDescriber)
    {
        var response = httpClient.GetAsync(httpJobDescriber.Url).Result;

        response.EnsureSuccessStatusCode();

        return response;
    }

    private HttpResponseMessage Post(HttpClient httpClient, HttpJobDescriber httpJobDescriber)
    {
        var requestPayload = new StringContent(httpJobDescriber.RequestPayload);
        var response = httpClient.PostAsync(httpJobDescriber.Url, requestPayload).Result;

        response.EnsureSuccessStatusCode();

        return response;
    }
    private HttpResponseMessage Put(HttpClient httpClient, HttpJobDescriber httpJobDescriber)
    {
        var requestPayload = new StringContent(httpJobDescriber.RequestPayload);
        var response = httpClient.PutAsync(httpJobDescriber.Url, requestPayload).Result;

        response.EnsureSuccessStatusCode();

        return response;
    }
    private HttpResponseMessage Delete(HttpClient httpClient, HttpJobDescriber httpJobDescriber)
    {
        var response = httpClient.DeleteAsync(httpJobDescriber.Url).Result;

        response.EnsureSuccessStatusCode();

        return response;
    }
}