using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirlyMonitor.Controllers
{
    [Route("/api/[controller]")]
    [Authorize]
    public class AliveController : Controller
    {
        private readonly ILogger<AliveController> _logger;

        public AliveController(ILogger<AliveController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("App is alive");
            return Ok("App is alive");
        }
    }
}
