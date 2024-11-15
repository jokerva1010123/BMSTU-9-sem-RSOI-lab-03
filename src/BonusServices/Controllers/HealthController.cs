using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers
{
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet("manage/health")]
        public ActionResult HealthCheck()
        {
            return Ok();
        }
    }
}
