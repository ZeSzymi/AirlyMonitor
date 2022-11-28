using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeasurementsSimulator.Controllers
{
    [Route("/api/[controller]")]
    [AllowAnonymous]
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
