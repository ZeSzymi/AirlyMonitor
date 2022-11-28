using AirlyInfrastructure.Models.Database;
using AirlyMonitor.Models.Database;

namespace AirlyInfrastructure.Repositories.Interfaces
{
    public interface IInstallationsRepository
    {
        Task<List<int>> GetInstallationIdsAsync();
        Task<Installation> AddInstallationAsync(Installation installation);
        Task<Installation?> GetInstallationAsync(int installationId);
        Task<UserInstallation> AddUserInstallationAsync(UserInstallation userInstallation);
        Task<UserInstallation?> GetUserInstallationAsync(string userId, int installationId);
        Task<List<Installation>> GetInstallationsForUserAsync(string userId);
    }
}
