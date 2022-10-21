using AirlyInfrastructure.Database;
using AirlyInfrastructure.Repositories.Interfaces;
using AirlyMonitor.Services.Interface;
using AirlyMonitor.Services.Interfaces;

namespace AirlyMonitor.Services
{
    public class AlertDefinitionsService : IAlertDefinitionsService
    {
        private readonly IInstallationRepository _installationRepository;
        private readonly IAlertDefinitionsRepository _alertDefinitionsRepository;
        private readonly IAirlyApiService _airlyApiService;

        public AlertDefinitionsService(IInstallationRepository installationRepository, IAlertDefinitionsRepository alertDefinitionsRepository, IAirlyApiService airlyApiService)
        {
            _installationRepository = installationRepository;
            _alertDefinitionsRepository = alertDefinitionsRepository;
            _airlyApiService = airlyApiService;
        }

        public async Task<AlertDefinition> AddAlertDefinitionAsync(AlertDefinition alertDefinition)
        {
            var installation = await _installationRepository.GetInstallationAsync(alertDefinition.InstallationId);
            if (installation == null)
            {
                var installationFromAirly = await _airlyApiService.GetInstallationByIdAsync(alertDefinition.InstallationId);
                 await _installationRepository.AddInstallationAsync(installationFromAirly);
            }

            alertDefinition.Deleted = false;
            return await _alertDefinitionsRepository.AddAlertDefinitionAsync(alertDefinition);
        }
    }
}
