﻿using Microsoft.Extensions.Logging;
using System.Text.Json;
using TGC.JobServer.Abstractions.Infrastructure;
using TGC.JobServer.Abstractions.Jobs;
using TGC.JobServer.Jobs.JobTypes.Containers;
using TGC.JobServer.Models;

namespace TGC.JobServer.Jobs;

public class OrderBioBagsJob : IInvokeableJob
{
    private readonly ILogger<OrderBioBagsJob> _logger;
    private readonly IStandardHttpClient _standardHttpClient;

    public OrderBioBagsJob(ILogger<OrderBioBagsJob> logger, IStandardHttpClient standardHttpClient)
    {
        _logger = logger;
        _standardHttpClient = standardHttpClient;
    }

    public bool Accept(string jobReference)
    {
        return jobReference == JobTypeReferences.ORDER_BIO_BAGS;
    }

    public void Execute(HangfireJobPayload hangfireJobPayload)
    {
        var orderBioBagsDescriber = JsonSerializer.Deserialize<OrderBioBagsDescriber>(hangfireJobPayload.JobTypeInformation);

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
            var myHttpClient = new HttpClient();
            var response = myHttpClient.PostAsync("https://nemaffaldsservice.kk.dk/BestilBioPoser/CreateRequest", formContent).Result;

            var responseContent = response.Content.ReadAsStringAsync().Result;

            _logger.LogInformation(responseContent);
        }
    }
}