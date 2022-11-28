using AirlyInfrastructure.Models.Database;
using AirlyMonitor.Models.Database;

namespace AirlyMonitor.Services.Interfaces
{
    public interface IInstallationsService
    {
        Task<Installation> AddInstallationIfDoesNotExistAsync(int installationId);
        Task<UserInstallation> AddUserInstallationIfDoesNotExistAsync(string userId, int installationId);
        Task<Installation> MarkInstallationAsync(string userId, int installationId);
        Task<List<Installation>> GetUserInstallations(string userId);
    }
}
