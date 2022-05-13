using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TGC.JobServer.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeartbeatController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok("Online");
        }
    }
}
