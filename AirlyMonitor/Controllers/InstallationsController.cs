using AirlyMonitor.Models.Dtos;
using AirlyMonitor.Models.QueryParams;
using AirlyMonitor.Services.Interface;
using AirlyMonitor.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirlyMonitor.Controllers
{
    [Route("/api/[controller]")]
    [Authorize]
    public class InstallationsController : Controller
    {
        private readonly IInstallationsService _installationsService;

        public InstallationsController(IInstallationsService installationsService)
        {
            _installationsService = installationsService;
        }

        [HttpPost("{installationId}")]
        public async Task<ActionResult<InstallationDto>> AddInstallationAsync(int installationId)
        {
            var installation = await _installationsService.AddInstallationIfDoesNotExistAsync(installationId);
            return Ok(installation);
        }

        [HttpPost("mark/{installationId}")]
        public async Task<ActionResult<InstallationDto>> MarkInstallationIdAsync(int installationId)
        {
            var installations = await _installationsService.MarkInstallationAsync(User.Identity.Name, installationId);
            return Ok(installations);
        }

        [HttpGet()]
        public async Task<ActionResult<List<InstallationDto>>> GetInstallationsAsync([FromQuery] GetInstallationsQueryParams queryParams)
        {
            var installations = await _installationsService.GetNearestInstallationsAsync(queryParams);
            return Ok(installations);
        }

        [HttpGet("user")]
        public async Task<ActionResult<List<InstallationDto>>> GetInstallationsForUserAsync()
        {
            var installations = await _installationsService.GetUserInstallations(User.Identity.Name);
            return Ok(installations);
        }
    }
}
