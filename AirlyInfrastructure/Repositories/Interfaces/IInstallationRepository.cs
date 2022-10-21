using AirlyInfrastructure.Database;
using AirlyMonitor.Models.Database;

namespace AirlyInfrastructure.Repositories.Interfaces
{
    public interface IInstallationRepository
    {
        Task<List<int>> GetInstallationIdsAsync();
        Task<Installation> AddInstallationAsync(Installation installation);
        Task<Installation?> GetInstallationAsync(int installationId);
    }
}
