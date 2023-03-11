using AirlyInfrastructure.Database;
using AirlyInfrastructure.Repositories.Interfaces;
using AirlyMonitor.Services.Interfaces;

namespace AirlyMonitor.Services
{
    public class AlertDefinitionsService : IAlertDefinitionsService
    {
        private readonly IAlertDefinitionsRepository _alertDefinitionsRepository;
        private readonly IInstallationsService _installationsService;

        public AlertDefinitionsService(IAlertDefinitionsRepository alertDefinitionsRepository, IInstallationsService installationsService)
        {
            _alertDefinitionsRepository = alertDefinitionsRepository;
            _installationsService = installationsService;
        }

        public async Task<AlertDefinition> AddAlertDefinitionAsync(AlertDefinition alertDefinition)
        {
            await _installationsService.AddInstallationIfDoesNotExistAsync(alertDefinition.InstallationId);
            await _installationsService.AddUserInstallationIfDoesNotExistAsync(alertDefinition.UserId, alertDefinition.InstallationId);
            alertDefinition.Deleted = false;
            return await _alertDefinitionsRepository.AddAlertDefinitionAsync(alertDefinition);
        }

        public async Task RemoveAlertDefinitionAsync(int installationId)
        {
            var alertDefinitions = await _alertDefinitionsRepository.GetAlertDefinitionsAsync(installationId);
            await _alertDefinitionsRepository.RemoveAlertDefinitions(alertDefinitions);
        }

        public Task<List<AlertDefinition>> GetAlertDefinitionsAsync(int installationId)
            => _alertDefinitionsRepository.GetAlertDefinitionsAsync(installationId);

        public Task<List<AlertDefinition>> GetAlertDefinitionsAsync()
            => _alertDefinitionsRepository.GetAlertDefinitionsAsync();

        public Task<List<AlertDefinition>> GetAlertDefinitionsForUserAsync(string userId)
            => _alertDefinitionsRepository.GetAlertDefinitionsForUserAsync(userId);
        
    }
}
