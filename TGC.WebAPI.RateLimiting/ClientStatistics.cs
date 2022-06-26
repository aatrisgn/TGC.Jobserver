﻿namespace TGC.WebAPI.RateLimiting;

public class ClientStatistics
{
    public DateTime LastSuccessfulResponseTime { get; set; }
    public int NumberOfRequestsCompletedSuccessfully { get; set; }
}
