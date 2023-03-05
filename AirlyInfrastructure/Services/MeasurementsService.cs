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

        public Task<List<Measurement>> GetMeasurementsAsync(int installationIds, DateTime? from, DateTime? to) {
            var fromDateTime = from ?? DateTime.UtcNow.AddDays(-1);
            var toDateTime = to ?? DateTime.UtcNow.AddDays(1);
            return _measurementRepository.GetMeasurementsAsync(installationIds, fromDateTime, toDateTime);
        }
             
    }
}
