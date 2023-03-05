using AirlyInfrastructure.Database;
using AirlyInfrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirlyMonitor.Controllers
{
    [Route("/api/[controller]")]
    [Authorize]
    public class AlertsController : Controller
    {
        private readonly IAlertsService _alertsService;

        public AlertsController(IAlertsService alertsService)
        {
            _alertsService = alertsService;
        }

        [HttpGet("{alertDefinitionId:guid}")]
        public async Task<ActionResult<List<Alert>>> GetAllAlerts(Guid alertDefinitionId)
        {
            var alertDefinitions = await _alertsService.GetAlertsForAlertDefinitionId(alertDefinitionId);
            return Ok(alertDefinitions);
        }

        [HttpGet("raised/{alertDefinitionId:guid}")]
        public async Task<ActionResult<List<Alert>>> GetRaisedAlerts(Guid alertDefinitionId)
        {
            var alertDefinitions = await _alertsService.GetRaisedAlertsForAlertDefinitionId(alertDefinitionId);
            return Ok(alertDefinitions);
        }
    }
}
