using AirlyInfrastructure.Models.Database;
using AirlyMonitor.Models.Database;

namespace AirlyInfrastructure.Repositories.Interfaces
{
    public interface IInstallationsRepository
    {
        Task<List<int>> GetInstallationIdsAsync();
        Task<Installation> AddInstallationAsync(Installation installation);
        Task<List<Installation>> AddInstallationsAsync(List<Installation> installation);
        Task<Installation?> GetInstallationAsync(int installationId);
        Task<UserInstallation> AddUserInstallationAsync(UserInstallation userInstallation);
        Task<UserInstallation?> GetUserInstallationAsync(string userId, int installationId);
        Task RemoveUserInstallation(string userId, int installationId);
        Task<List<Installation>> GetInstallationsForUserAsync(string userId);
        Task<List<Installation>> GetInstallationsAsync(List<int> installationIds);
        Task<List<int>> GetUserInstallationIds(string userId, List<int> installationIds);
        Task<List<int>> GetInstallationsIdsForUserAlertDefinitions(string userId, List<int> installationIds);
        Task<bool> IsMarked(string userId, int installationId);
        Task<bool> HasAlert(string userId, int installationId);
    }
}
