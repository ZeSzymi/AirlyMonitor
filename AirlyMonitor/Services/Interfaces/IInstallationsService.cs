using AirlyInfrastructure.Models.Database;
using AirlyMonitor.Models.Dtos;
using AirlyMonitor.Models.QueryParams;

namespace AirlyMonitor.Services.Interfaces
{
    public interface IInstallationsService
    {
        Task<InstallationDto> AddInstallationIfDoesNotExistAsync(int installationId);
        Task<UserInstallation> AddUserInstallationIfDoesNotExistAsync(string userId, int installationId);
        Task<InstallationDto> MarkInstallationAsync(string userId, int installationId);
        Task<List<InstallationDto>> GetNearestInstallationsAsync(GetInstallationsQueryParams queryParams);
        Task<List<InstallationDto>> GetUserInstallations(string userId);
    }
}
