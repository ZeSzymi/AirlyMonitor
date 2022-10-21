using AirlyInfrastructure.Database;

namespace AirlyInfrastructure.Repositories.Interfaces
{
    public interface IMeasurementRepository
    {
        Task<List<Measurement>> AddMeasurementsAsync(List<Measurement> measurements);
    }
}
