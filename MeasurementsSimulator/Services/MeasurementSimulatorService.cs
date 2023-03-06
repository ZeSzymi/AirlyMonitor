using AirlyInfrastructure.Repositories.Interfaces;
using MeasurementsSimulator.Services.Interfaces;
using Newtonsoft.Json;

namespace MeasurementsSimulator.Services
{
    public class MeasurementSimulatorService : IMeasurementSimulatorService
    {
        private readonly IMeasurementGenerationService _measurementGenerationService;
        private readonly IMeasurementRepository _measurementRepository;
        private readonly IInstallationsRepository _installationRepository;
        private readonly ILogger<MeasurementSimulatorService> _logger;

        public MeasurementSimulatorService(
            IMeasurementGenerationService measurementGenerationService, 
            IMeasurementRepository measurementRepository,
            IInstallationsRepository installationRepository,
            ILogger<MeasurementSimulatorService> logger)
        {
            _measurementGenerationService = measurementGenerationService;
            _measurementRepository = measurementRepository;
            _installationRepository = installationRepository;
            _logger = logger;
        }

        public async Task SimulateAsync(DateTime now)
        {
            try
            {
                var installationIds = (await _installationRepository.GetInstallationIdsAsync()).ToList();
                _logger.LogInformation($"Generating measurements for installations: {installationIdsToString(installationIds)}");

                var measurements = _measurementGenerationService.GenerateMeasurements(installationIds, now);
                _logger.LogInformation($"Generated measurements: {JsonConvert.SerializeObject(measurements)}");

                await _measurementRepository.AddMeasurementsAsync(measurements);
                _logger.LogInformation($"Measurements added to database succesfully");
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message); 
            }
        }

        private string installationIdsToString(List<int> installationIds)
            => installationIds.Select(installationId => installationId.ToString()).Aggregate((a, b) => $"{a} | {b} |");
    }
}
