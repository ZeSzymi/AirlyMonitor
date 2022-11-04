using AirlyMonitor.Models.Dtos;
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
        public async Task<IActionResult> AddAlertDefinition([FromBody] AlertDefinitionDto alertDefinition)
        {
            var addedAlertDefinition = await _alertDefinitionsService.AddAlertDefinitionAsync(alertDefinition.ToAlertDefinition());
            return Ok(addedAlertDefinition);
        }

        [HttpGet("{installationId}")]
        public async Task<IActionResult> GetAlertDefinitions(int installationId)
        {
            var alertDefinitions = await _alertDefinitionsService.GetAlertDefinitionsAsync(installationId);
            return Ok(alertDefinitions);
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllAlertDefinitions()
        {
            var alertDefinitions = await _alertDefinitionsService.GetAlertDefinitionsAsync();
            return Ok(alertDefinitions);
        }
    }
}
