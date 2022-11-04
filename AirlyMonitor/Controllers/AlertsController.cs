using AirlyInfrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AirlyMonitor.Controllers
{
    [Route("/api/[controller]")]
    public class AlertsController : Controller
    {
        private readonly IAlertsService _alertsService;

        public AlertsController(IAlertsService alertsService)
        {
            _alertsService = alertsService;
        }

        [HttpGet("{alertDefinitionId:guid}")]
        public async Task<IActionResult> GetAlerts(Guid alertDefinitionId)
        {
            var alertDefinitions = await _alertsService.GetAlertsForAlertDefinitionId(alertDefinitionId);
            return Ok(alertDefinitions);
        }
    }
}
