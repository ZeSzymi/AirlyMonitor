using AirlyInfrastructure.Database;

namespace AirlyInfrastructure.Repositories.Interfaces
{
    public interface IMeasurementRepository
    {
        Task<List<Measurement>> AddMeasurementsAsync(List<Measurement> measurements);
        Task<List<Measurement>> GetMeasurementsAsync(List<int> installationsIds);
        Task<List<Measurement>> GetMeasurementsAsync(int installationId);
        Task<List<Measurement?>> GetLatestMeasurementsAsync(List<int> installationIds);
    }
}
