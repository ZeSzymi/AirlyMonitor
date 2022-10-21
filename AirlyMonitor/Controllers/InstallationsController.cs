using AirlyMonitor.Models.Constants;
using AirlyMonitor.Models.Database;
using AirlyMonitor.Models.QueryParams;
using AirlyMonitor.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AirlyMonitor.Controllers
{
    [Route("/api/[controller]")]
    public class InstallationsController : Controller
    {
        private readonly IHttpService _httpService;

        public InstallationsController(IHttpService httpService)
        {
            _httpService = httpService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetInstallationsAsync([FromQuery] GetInstallationsQueryParams queryParams)
        {
            var installations = await _httpService.Get<List<Installation>>($"https://airapi.airly.eu/v2/{AirlyApi.NearestInstallationsUrl(queryParams.Lat, queryParams.Lng, queryParams.MaxDistanceKM, queryParams.MaxResults)}");
            return Ok(installations);
        }
    }
}
