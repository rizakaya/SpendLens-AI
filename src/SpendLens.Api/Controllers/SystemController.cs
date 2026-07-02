using Microsoft.AspNetCore.Mvc;

namespace SpendLens.Api.Controllers;

[ApiController]
[Route("api/system")]
public sealed class SystemController : ControllerBase
{
    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new
        {
            status = "ok",
            service = "SpendLens.Api",
            utcTime = DateTimeOffset.UtcNow,
            version = "mvp"
        });
    }
}
