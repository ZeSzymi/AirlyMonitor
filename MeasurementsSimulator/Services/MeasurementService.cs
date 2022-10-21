using AirlyInfrastructure.Database;
using AirlyInfrastructure.Repositories.Interfaces;
using MeasurementsSimulator.Services.Interfaces;

namespace MeasurementsSimulator.Services
{
    public class MeasurementService : IMeasurementService
    {
        private readonly IMeasurementGenerationService _measurementGenerationService;
        private readonly IAlertDefinitionsRepository _alertDefinitionsRepository;
        private readonly IMeasurementRepository _measurementRepository;

        public MeasurementService(IMeasurementGenerationService measurementGenerationService, IAlertDefinitionsRepository alertDefinitionsRepository, IMeasurementRepository measurementRepository)
        {
            _measurementGenerationService = measurementGenerationService;
            _alertDefinitionsRepository = alertDefinitionsRepository;
            _measurementRepository = measurementRepository;
        }

        public async Task<List<Measurement>> AddMeasurementsAsync(DateTime utcNow)
        {
            var installationIds = (await _alertDefinitionsRepository.GetAlertDefinitions()).Select(a => a.InstallationId).ToList();
            var measurements = _measurementGenerationService.GenerateMeasurements(installationIds, utcNow);
            return await _measurementRepository.AddMeasurementsAsync(measurements);
        }
    }
}
