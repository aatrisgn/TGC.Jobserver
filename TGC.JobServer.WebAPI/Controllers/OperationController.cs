using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Reflection;
using TGC.WebAPI.RateLimiting;

namespace TGC.JobServer.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OperationController : ControllerBase
{
    private readonly IDistributedCache _distributedCache;
    private readonly IEnumerable<EndpointDataSource> _endpointSources;

    public OperationController(IDistributedCache distributedCache, IEnumerable<EndpointDataSource> endpointSources)
    {
        _distributedCache = distributedCache;
        _endpointSources = endpointSources;
    }

    [HttpGet("Heartbeat")]
    public async Task<IActionResult> Heartbeat()
    {
        return Ok("Online");
    }

    [HttpGet("Version")]
    public async Task<IActionResult> Version()
    {
        var assembly = Assembly.GetExecutingAssembly().GetName().Version;
        return Ok(assembly);
    }

    [LimitRequests(MaxRequests = 5, TimeWindow = 1)]
    [HttpGet("Throttles")]
    public async Task<IActionResult> GetThrottles()
    {
        var endpointList = new List<string>();
        var cacheList = new List<ClientStatistics>();

        foreach (var endpoint in _endpointSources)
        {
            foreach(var subEndpoint in endpoint.Endpoints)
            {
                var decorator = subEndpoint?.Metadata.GetMetadata<LimitRequests>();

                if (decorator != null)
                {
                    endpointList.Add($"{((RouteEndpoint)subEndpoint).RoutePattern.RawText}_{Request.HttpContext.Connection.RemoteIpAddress.ToString()}");
                }
            }
        }

        //foreach(var endpoint in endpointList)
        //{
        //    var cacheThrottle = _cache.
        //} TODO: Finish building possibility for querying throttle

        return Ok(endpointList);
    }
}
