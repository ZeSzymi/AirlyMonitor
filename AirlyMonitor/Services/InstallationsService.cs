using AirlyInfrastructure.Models.Database;
using AirlyInfrastructure.Repositories.Interfaces;
using AirlyMonitor.Models.Database;
using AirlyMonitor.Services.Interface;
using AirlyMonitor.Services.Interfaces;

namespace AirlyMonitor.Services
{
    public class InstallationsService : IInstallationsService
    {
        private readonly IInstallationsRepository _installationsRepository;
        private readonly IAirlyApiService _airlyApiService;

        public InstallationsService(IInstallationsRepository installationsRepository, IAirlyApiService airlyApiService)
        {
            _installationsRepository = installationsRepository;
            _airlyApiService = airlyApiService;
        }

        public async Task<Installation> AddInstallationIfDoesNotExistAsync(int installationId)
        {
            var installation = await _installationsRepository.GetInstallationAsync(installationId);
            if (installation == null)
            {
                installation = await _airlyApiService.GetInstallationByIdAsync(installationId);
                await _installationsRepository.AddInstallationAsync(installation);
            }

            return installation;
        }

        public async Task<Installation> MarkInstallationAsync(string userId, int installationId)
        {
            var installation = await AddInstallationIfDoesNotExistAsync(installationId);
            await AddUserInstallationIfDoesNotExistAsync(userId, installationId);

            return installation;
        }

        public async Task<UserInstallation> AddUserInstallationIfDoesNotExistAsync(string userId, int installationId)
        {
            var userInstallation = await _installationsRepository.GetUserInstallationAsync(userId, installationId);

            if (userInstallation == null)
            {
                userInstallation = new UserInstallation { InstallationId = installationId, UserId = userId };
                await _installationsRepository.AddUserInstallationAsync(userInstallation);
            }

            return userInstallation;
        }

        public Task<List<Installation>> GetUserInstallations(string userId)
            => _installationsRepository.GetInstallationsForUserAsync(userId); 

    }
}
