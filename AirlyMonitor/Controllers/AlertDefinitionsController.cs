using AirlyMonitor.Models.Dtos;
using AirlyMonitor.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirlyMonitor.Controllers
{
    [Route("/api/[controller]")]
    [Authorize]
    public class AlertDefinitionsController : Controller
    {
        private readonly IAlertDefinitionsService _alertDefinitionsService;

        public AlertDefinitionsController(IAlertDefinitionsService alertDefinitionsService)
        {
            _alertDefinitionsService = alertDefinitionsService;
        }

        [HttpPost()]
        public async Task<ActionResult<AlertDefinitionDto>> AddAlertDefinition([FromBody] AlertDefinitionDto alertDefinition)
        {
            var addedAlertDefinition = await _alertDefinitionsService.AddAlertDefinitionAsync(alertDefinition.ToAlertDefinition(User.Identity.Name));
            return Ok(addedAlertDefinition);
        }

        [HttpDelete("{installationId}")]
        public async Task<ActionResult<AlertDefinitionDto>> RemoveAlertDefinition(int installationId)
        {
            await _alertDefinitionsService.RemoveAlertDefinitionAsync(installationId);
            return Ok();
        }

        [HttpGet("{installationId}")]
        public async Task<ActionResult<List<AlertDefinitionDto>>> GetAlertDefinitions(int installationId)
        {
            var alertDefinitions = await _alertDefinitionsService.GetAlertDefinitionsAsync(installationId);
            return Ok(alertDefinitions);
        }

        [HttpGet("user")]
        public async Task<ActionResult<List<AlertDefinitionDto>>> GetAlertDefinitions()
        {
            var alertDefinitions = await _alertDefinitionsService.GetAlertDefinitionsForUserAsync(User.Identity.Name);
            return Ok(alertDefinitions);
        }

        [HttpGet()]
        public async Task<ActionResult<List<AlertDefinitionDto>>> GetAllAlertDefinitions()
        {
            var alertDefinitions = await _alertDefinitionsService.GetAlertDefinitionsAsync();
            return Ok(alertDefinitions);
        }
    }
}
