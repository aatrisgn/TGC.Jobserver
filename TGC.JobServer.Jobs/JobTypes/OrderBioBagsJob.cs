using TGC.JobServer.Abstractions.Jobs;
using TGC.JobServer.Models;
using TGC.JobServer.Models.DTOs;

namespace TGC.JobServer.Jobs;

public class OrderBioBagsJob : IInvokeableJob, IInitializeOnStartup
{
    public bool Accept(string jobReference)
    {
        return jobReference == JobTypeReferences.ORDER_BIO_BAGS;
    }

    public void Execute(HangfireJobPayload jobRequest)
    {
        var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("name", "Asger Thyregod"),
                new KeyValuePair<string, string>("addressId", "6c913f09-b17c-e911-bfa7-005056ad66a0"),
                new KeyValuePair<string, string>("causeId", "76643848-957b-e911-bfa7-005056ad66a0"),
                new KeyValuePair<string, string>("outputId", "6bb00e40-947b-e911-bfa7-005056ad66a0"),
                new KeyValuePair<string, string>("email", "asger.thyregod@gmail.com"),
            });

        var myHttpClient = new HttpClient();
        var response = myHttpClient.PostAsync("https://nemaffaldsservice.kk.dk/BestilBioPoser/CreateRequest", formContent).Result;

        var some = response.Content.ReadAsStringAsync().Result;
        Console.WriteLine(some);
    }
    public void InitializeOnStartup()
    {
        throw new NotImplementedException();
    }
}