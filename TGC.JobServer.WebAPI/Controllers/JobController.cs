using Microsoft.AspNetCore.Mvc;
using TGC.JobServer.Abstractions.Services;
using TGC.JobServer.Models.DTOs;
using TGC.WebAPI.RateLimiting;

namespace TGC.JobServer.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JobController : ControllerBase
{
    private readonly IJobService _jobService;
    public JobController(IJobService jobService)
    {
        _jobService = jobService;
    }

    // GET: api/<JobController>
    [HttpGet("startup")]
    public async Task<IActionResult> GetStartupJobs()
    {
        var startupJobIds = await Task.Factory.StartNew(() => {
            return _jobService.GetStartupJobIds();
        });

        return Ok(new JobResponse(startupJobIds));
    }

    [HttpGet("{jobId}")]
    public async Task<IActionResult> Get(int jobId)
    {
        var jobStatusDto = await Task.Factory.StartNew(() => {
            return _jobService.GetJobStatusById(jobId.ToString());
        });

        var jobInformationResponse = new JobInformationResponse(jobStatusDto, jobId);

        return Ok(jobInformationResponse);
    }

    [HttpGet("history/{jobId}")]
    public async Task<IActionResult> GetHistory(int jobId)
    {
        var jobStatusDto = await Task.Factory.StartNew(() => {
            return _jobService.GetJobStatusById(jobId.ToString());
        });

        var jobHistoryResponse = new JobHistoryResponse(jobStatusDto, jobId);

        return Ok(jobHistoryResponse);
    }

    [LimitRequests(MaxRequests = 5, TimeWindow = 1)]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] List<JobRequest> jobRequests)
    {
        if(jobRequests == null)
        {
            return BadRequest();
        }

        if(jobRequests.Count > 10)
        {
            return BadRequest("You are not allowed to create more than 10 jobs at a time");
        }

        var jobIds = await Task.Factory.StartNew(() => {
            return _jobService.HandleJobs(jobRequests);
        });

        return Ok(new JobResponse(jobIds));
    }

    [LimitRequests(MaxRequests = 5, TimeWindow = 1)]
    [HttpPut]
    public async Task<IActionResult> Put([FromBody]JobRequest jobRequest)
    {
        if (jobRequest == null)
        {
            return BadRequest();
        }


        var jobIds = await Task.Factory.StartNew(() => {
            return _jobService.HandleJob(jobRequest);
        });
          
        return Ok(jobIds);
    }

    [LimitRequests(MaxRequests = 5, TimeWindow = 1)]
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        var succesfulDeletion = _jobService.DeleteJob(id);
        if (succesfulDeletion)
        {
            return Ok();
        }

        return NotFound();
    }
}
