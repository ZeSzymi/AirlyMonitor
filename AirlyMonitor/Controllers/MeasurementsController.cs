using AirlyInfrastructure.Models.Constants;
using AirlyInfrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AirlyMonitor.Controllers
{
    [Route("/api/[controller]")]
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

        [HttpGet("{instrumentId}")]
        public async Task<IActionResult> GetMeasurements(int installationId)
        {
            var measurements = _measurementsService.GetMeasurementsAsync(installationId);
            return Ok(measurements);
        }
    }
}
