using AirlyInfrastructure.Database;
using AirlyInfrastructure.Repositories.Interfaces;
using AirlyInfrastructure.Services.Interfaces;

namespace AirlyInfrastructure.Services
{
    public class MeasurementsService : IMeasurementsService
    {
        private readonly IMeasurementRepository _measurementRepository;

        public MeasurementsService(IMeasurementRepository measurementRepository)
        {
            _measurementRepository = measurementRepository;
        }

        public Task<List<Measurement>> GetMeasurementsAsync(List<int> installationIds) =>
             _measurementRepository.GetMeasurementsAsync(installationIds);

        public Task<List<Measurement>> GetMeasurementsAsync(int installationIds) =>
             _measurementRepository.GetMeasurementsAsync(installationIds);
    }
}
