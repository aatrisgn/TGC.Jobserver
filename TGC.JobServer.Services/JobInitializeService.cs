using System.Text.Json;
using TGC.JobServer.Abstractions.Infrastructure;
using TGC.JobServer.Abstractions.Services;
using TGC.JobServer.Models.DTOs;

namespace TGC.JobServer.Services;
public class JobInitializeService : IJobInitializeService
{
    private readonly IJobService _jobService;
    private readonly ICustomMonitoringApi _customMonitoringApi;

    public JobInitializeService(IJobService jobService, ICustomMonitoringApi customMonitoringApi)
    {
        _jobService = jobService;
        _customMonitoringApi = customMonitoringApi;
    }

    public async Task InitialzeJobsOnStartup()
    {
        var startupJobsConfigurationFilePath = "JobInitializer.json";
        var startUpJobsJson = await File.ReadAllTextAsync(startupJobsConfigurationFilePath);

        var startupJobs = JsonSerializer.Deserialize<List<JobRequest>>(startUpJobsJson);

        var jobIds = _jobService.HandleJobs(startupJobs);

        foreach(var jobId in jobIds)
        {
            _customMonitoringApi.AddJobIdInitializedOnStartup(jobId);
        }
    }
}
