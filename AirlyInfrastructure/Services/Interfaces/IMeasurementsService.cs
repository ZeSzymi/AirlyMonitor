using AirlyInfrastructure.Database;

namespace AirlyInfrastructure.Services.Interfaces
{
    public interface IMeasurementsService
    {
        Task<List<Measurement>> GetMeasurementsAsync(List<int> installationIds);
        Task<List<Measurement>> GetMeasurementsAsync(int installationIds, DateTime? from, DateTime? to);
    }
}
