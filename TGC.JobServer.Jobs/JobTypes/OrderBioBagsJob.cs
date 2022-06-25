using Hangfire.Server;
using Microsoft.Extensions.Logging;
using TGC.JobServer.Abstractions.Infrastructure;
using TGC.JobServer.Abstractions.Jobs;
using TGC.JobServer.Jobs.JobTypes.Containers;
using TGC.JobServer.Models;

namespace TGC.JobServer.Jobs;

public class OrderBioBagsJob : IInvokeableJob
{
    private readonly ILogger<OrderBioBagsJob> _logger;
    private readonly IStandardHttpClient _standardHttpClient;
    private readonly IJsonSerializer _jsonSerializer;

    public OrderBioBagsJob(ILogger<OrderBioBagsJob> logger, IStandardHttpClient standardHttpClient, IJsonSerializer jsonSerializer)
    {
        _logger = logger;
        _standardHttpClient = standardHttpClient;
        _jsonSerializer = jsonSerializer;
    }

    public bool Accept(string jobReference)
    {
        return jobReference == JobTypeReferences.ORDER_BIO_BAGS;
    }

    public void Callback(string url, ICallbackResponse jobCallbackResponse)
    {
        throw new NotImplementedException();
    }

    public void Execute(HangfireJobPayload hangfireJobPayload, PerformContext context)
    {
        var orderBioBagsDescriber = _jsonSerializer.Deserialize<OrderBioBagsDescriber>(hangfireJobPayload.JobTypeInformation);

        var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("name", orderBioBagsDescriber.Name),
                new KeyValuePair<string, string>("addressId", orderBioBagsDescriber.AdressId.ToString()),
                new KeyValuePair<string, string>("causeId",  orderBioBagsDescriber.CauseId.ToString()),
                new KeyValuePair<string, string>("outputId", orderBioBagsDescriber.OutputId.ToString()),
                new KeyValuePair<string, string>("email", orderBioBagsDescriber.Email),
            });


        using(var httpClient = _standardHttpClient.CreateClient())
        {
            var response = httpClient.PostAsync("https://nemaffaldsservice.kk.dk/BestilBioPoser/CreateRequest", formContent).Result;

            var responseContent = response.Content.ReadAsStringAsync().Result;

            _logger.LogInformation(responseContent);
        }
    }
}