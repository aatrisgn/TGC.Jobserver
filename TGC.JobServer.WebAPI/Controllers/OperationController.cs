using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace TGC.JobServer.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OperationController : ControllerBase
{
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
}
