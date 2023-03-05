using AirlyInfrastructure.Database;
using AirlyInfrastructure.Models.Constants;
using AirlyInfrastructure.Services.Interfaces;
using AirlyMonitor.Models.QueryParams;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirlyMonitor.Controllers
{
    [Route("/api/[controller]")]
    [Authorize]
    public class MeasurementsController : Controller
    {
        private readonly IMeasurementsService _measurementsService;

        public MeasurementsController(IMeasurementsService measurementsService)
        {
            _measurementsService = measurementsService;
        }

        [HttpGet("names")]
        public IActionResult GetMeasurementNames()
        {
            return Ok(new string[]
            {
                MeasurementValueNames.PM1,
                MeasurementValueNames.PM10,
                MeasurementValueNames.PM25,
                MeasurementValueNames.PRESSURE,
                MeasurementValueNames.HUMIDITY,
                MeasurementValueNames.TEMPERATURE,
                MeasurementValueNames.NO2,
                MeasurementValueNames.O3
            });
        }

        [HttpGet("{installationId}")]
        public async Task<ActionResult<List<Measurement>>> GetMeasurements(int installationId, [FromQuery] GetMeasurementsQueryParams queryParams)
        {
            var measurements = await _measurementsService.GetMeasurementsAsync(installationId, queryParams.From, queryParams.To);
            return Ok(measurements);
        }
    }
}
