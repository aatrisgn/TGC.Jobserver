using System.Text.Json;
using TGC.JobServer.Abstractions.Infrastructure;
using TGC.JobServer.Abstractions.Services;
using TGC.JobServer.Models.DTOs;

namespace TGC.JobServer.Services;
public class JobInitializeService : IJobInitializeService
{
    private readonly IJobService _jobService;
    private readonly ICustomMonitoringApi _customMonitoringApi;
    private readonly IJsonSerializer _jsonSerializer;

    public JobInitializeService(IJobService jobService, ICustomMonitoringApi customMonitoringApi, IJsonSerializer jsonSerializer)
    {
        _jobService = jobService;
        _customMonitoringApi = customMonitoringApi;
        _jsonSerializer = jsonSerializer;
    }

    public async Task InitialzeJobsOnStartup()
    {
        var startupJobsConfigurationFilePath = "JobInitializer.json";
        var startUpJobsJson = await File.ReadAllTextAsync(startupJobsConfigurationFilePath);

        var startupJobs = _jsonSerializer.Deserialize<List<JobRequest>>(startUpJobsJson);

        var jobIds = _jobService.HandleJobs(startupJobs);

        foreach(var jobId in jobIds)
        {
            _customMonitoringApi.AddJobIdInitializedOnStartup(jobId);
        }
    }
}
