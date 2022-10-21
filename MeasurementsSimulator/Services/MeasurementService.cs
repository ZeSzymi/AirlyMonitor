using AirlyInfrastructure.Database;
using MeasurementsSimulator.Repositories.Interfaces;
using MeasurementsSimulator.Services.Interfaces;

namespace MeasurementsSimulator.Services
{
    public class MeasurementService : IMeasurementService
    {
        private readonly IMeasurementGenerationService _measurementGenerationService;
        private readonly IMeasurementRepository _measurementRepository;

        public MeasurementService(IMeasurementGenerationService measurementGenerationService, IMeasurementRepository measurementRepository)
        {
            _measurementGenerationService = measurementGenerationService;
            _measurementRepository = measurementRepository;
        }

        public async Task<List<Measurement>> AddMeasurementsAsync(DateTime utcNow)
        {
            var installationIds = await _measurementRepository.GetInstallationIdsAsync();
            var measurements = _measurementGenerationService.GenerateMeasurements(installationIds, utcNow);
            return await _measurementRepository.AddMeasurementsAsync(measurements);
        }
    }
}
