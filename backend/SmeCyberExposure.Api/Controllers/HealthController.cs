using Microsoft.AspNetCore.Mvc;

namespace SME.Exposure.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                status = "Healthy",
                service = "SME Cyber Exposure API",
                timestamp = DateTime.UtcNow
            });
        }
    }
}
