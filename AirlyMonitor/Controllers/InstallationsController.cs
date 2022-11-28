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
        private readonly IAirlyApiService _airlyApiService;
        private readonly IInstallationsService _installationsService;

        public InstallationsController(IAirlyApiService airlyApiService, IInstallationsService installationsService)
        {
            _airlyApiService = airlyApiService;
            _installationsService = installationsService;
        }

        [HttpPost("{installationId}")]
        public async Task<IActionResult> AddInstallationAsync(int installationId)
        {
            var installations = await _installationsService.AddInstallationIfDoesNotExistAsync(installationId);
            return Ok(installations);
        }

        [HttpPost("mark/{installationId}")]
        public async Task<IActionResult> MarkInstallationIdAsync(int installationId)
        {
            var installations = await _installationsService.MarkInstallationAsync(User.Identity.Name, installationId);
            return Ok(installations);
        }

        [HttpGet()]
        public async Task<IActionResult> GetInstallationsAsync([FromQuery] GetInstallationsQueryParams queryParams)
        {
            var installations = await _airlyApiService.GetNearestInstallationsAsync(queryParams);
            return Ok(installations);
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetInstallationsForUserAsync()
        {
            var installations = await _installationsService.GetUserInstallations(User.Identity.Name);
            return Ok(installations);
        }
    }
}
