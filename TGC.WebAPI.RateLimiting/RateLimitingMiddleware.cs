using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using System.Net;
using System.Text.Json;

namespace TGC.WebAPI.RateLimiting
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IDistributedCache _cache;

        public RateLimitingMiddleware(RequestDelegate next, IDistributedCache cache)
        {
            _next = next;
            _cache = cache;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            var decorator = endpoint?.Metadata.GetMetadata<LimitRequests>();

            if (decorator is null)
            {
                await _next(context);
                return;
            }

            var requestKey = GenerateClientKey(context);

            var statisticsFound = TryGetClientStatisticsByKey(requestKey, out var clientStatistics);

            if (statisticsFound == false || ClientCanPerformRequest(clientStatistics, decorator))
            {
                await UpdateClientStatisticsStorage(requestKey, clientStatistics, decorator);
                await _next(context);
            }

            context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
        }

        private static string GenerateClientKey(HttpContext context) => $"{context.Request.Path}_{context.Connection.RemoteIpAddress}";

        private bool TryGetClientStatisticsByKey(string key, out ClientStatistics value)
        {
            var cachedValue = _cache.GetString(key);
            value = default;

            if (cachedValue != null)
            {
                value = JsonSerializer.Deserialize<ClientStatistics>(cachedValue);
                return true;
            }

            return false;
        }

        private bool ClientCanPerformRequest(ClientStatistics clientStatistics, LimitRequests decorator)
        {
            var timespanExpired = DateTime.UtcNow > clientStatistics.LastSuccessfulResponseTime.AddSeconds(decorator.TimeWindow);
            var maxRequestReached = clientStatistics.NumberOfRequestsCompletedSuccessfully >= decorator.MaxRequests;

            return (maxRequestReached == false || timespanExpired);
        }

        private async Task UpdateClientStatisticsStorage(string key, ClientStatistics clientStatistics, LimitRequests decorator)
        {
            if(clientStatistics != null)
            {
                var secondsSinceLastSuccesfulRequest = DateTime.UtcNow - clientStatistics.LastSuccessfulResponseTime;

                if (secondsSinceLastSuccesfulRequest.TotalSeconds >= decorator.TimeWindow)
                {
                    clientStatistics.LastSuccessfulResponseTime = DateTime.UtcNow;
                    clientStatistics.NumberOfRequestsCompletedSuccessfully = 0;
                }

                clientStatistics.NumberOfRequestsCompletedSuccessfully +=1;
            }
            else
            {
                clientStatistics = new ClientStatistics
                {
                    LastSuccessfulResponseTime = DateTime.UtcNow,
                    NumberOfRequestsCompletedSuccessfully = 1
                };
            }
            
            var serializedClientStatistics = JsonSerializer.Serialize(clientStatistics);

            await _cache.SetStringAsync(key, serializedClientStatistics);

        }

    }

    public static class RequestCultureMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestThrottling(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RateLimitingMiddleware>();
        }
    }

}
