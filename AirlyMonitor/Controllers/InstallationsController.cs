using AirlyMonitor.Models.QueryParams;
using AirlyMonitor.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AirlyMonitor.Controllers
{
    [Route("/api/[controller]")]
    public class InstallationsController : Controller
    {
        private readonly IAirlyApiService _airlyApiService;

        public InstallationsController(IAirlyApiService airlyApiService)
        {
            _airlyApiService = airlyApiService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetInstallationsAsync([FromQuery] GetInstallationsQueryParams queryParams)
        {
            var installations = await _airlyApiService.GetNearestInstallationsAsync(queryParams);
            return Ok(installations);
        }
    }
}
