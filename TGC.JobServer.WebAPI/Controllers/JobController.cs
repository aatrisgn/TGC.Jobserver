using Microsoft.AspNetCore.Mvc;
using TGC.JobServer.Abstractions.Jobs;
using TGC.JobServer.Abstractions.Services;
using TGC.JobServer.Models.DTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TGC.JobServer.WebAPI.Controllers
{
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
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<JobController>/5
        [HttpGet("{jobId}")]
        public async Task<IActionResult> Get(int jobId)
        {
            var jobStatusDto = await Task.Factory.StartNew(() => {
                return _jobService.GetJobStatusById(jobId);
            });

            var jobInformationResponse = new JobInformationResponse(jobStatusDto, jobId);

            return Ok(jobInformationResponse);
        }

        [HttpGet("history/{jobId}")]
        public async Task<IActionResult> GetHistory(int jobId)
        {
            var jobStatusDto = await Task.Factory.StartNew(() => {
                return _jobService.GetJobStatusById(jobId);
            });

            var jobHistoryResponse = new JobHistoryResponse(jobStatusDto, jobId);

            return Ok(jobHistoryResponse);
        }

        // POST api/<JobController>
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

            return Ok(jobIds);
        }

        // PUT api/<JobController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            throw new NotImplementedException();
        }

        // DELETE api/<JobController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
