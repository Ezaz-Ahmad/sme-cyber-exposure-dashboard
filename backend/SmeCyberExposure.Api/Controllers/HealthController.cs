using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SmeCyberExposure.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    // Public endpoint (no token required)
    [HttpGet("public")]
    public IActionResult Public()
    {
        return Ok(new
        {
            status = "Healthy",
            service = "SME Cyber Exposure API",
            access = "public",
            timestamp = DateTime.UtcNow
        });
    }

    // Secured endpoint (token required)
    [Authorize]
    [HttpGet("secure")]
    public IActionResult Secure()
    {
        return Ok(new
        {
            status = "Healthy",
            service = "SME Cyber Exposure API",
            access = "authorized",
            user = User.Identity?.Name,
            timestamp = DateTime.UtcNow
        });
    }
}
