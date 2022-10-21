using AirlyInfrastructure.Database;
using AirlyMonitor.Models.Database;

namespace AirlyInfrastructure.Repositories.Interfaces
{
    public interface IMeasurementRepository
    {
        Task<List<Measurement>> AddMeasurementsAsync(List<Measurement> measurements);
        Task<List<int>> GetInstallationIdsAsync();
        Task<Installation> AddInstallationAsync(Installation installation);
    }
}
