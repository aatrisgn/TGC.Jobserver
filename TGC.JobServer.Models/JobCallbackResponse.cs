using System.Net;

namespace TGC.JobServer.Models;

public class JobCallbackResponse : ICallbackResponse
{
    public string JobId { get; set; }
    public HttpStatusCode HttpResponseCode { get; set; }
}
