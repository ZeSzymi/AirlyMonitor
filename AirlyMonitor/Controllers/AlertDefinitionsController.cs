using AirlyInfrastructure.Database;
using AirlyMonitor.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AirlyMonitor.Controllers
{
    [Route("/api/[controller]")]
    public class AlertDefinitionsController : Controller
    {
        private readonly IAlertDefinitionsService _alertDefinitionsService;

        public AlertDefinitionsController(IAlertDefinitionsService alertDefinitionsService)
        {
            _alertDefinitionsService = alertDefinitionsService;
        }

        [HttpPost()]
        public async Task<IActionResult> AddAlertDefinition([FromBody] AlertDefinition alertDefinition)
        {
            var addedAlertDefinition = await _alertDefinitionsService.AddAlertDefinitionAsync(alertDefinition);
            return Ok(addedAlertDefinition);
        }
    }
}
