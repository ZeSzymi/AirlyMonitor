using AirlyInfrastructure.Database;
using AirlyInfrastructure.Repositories.Interfaces;
using AirlyInfrastructure.Services.Interfaces;

namespace AirlyInfrastructure.Services
{
    public class AlertsService : IAlertsService
    {
        private readonly IAlertsRepository _alertsRepository;

        public AlertsService(IAlertsRepository alertsRepository)
        {
            _alertsRepository = alertsRepository;
        }

        public Task<List<Alert>> GetLatestAlertsAsync(List<Guid> alertDefinitionIds) =>
            _alertsRepository.GetLatestAlertsAsync(alertDefinitionIds);

        public Task<List<Alert>> GetAlertsForAlertDefinitionId(Guid alarmDefinitionId) =>
            _alertsRepository.GetAlertsForAlertDefinitionId(alarmDefinitionId);

        public Task<List<Alert>> GetRaisedAlertsForAlertDefinitionId(Guid alarmDefinitionId) =>
            _alertsRepository.GetRaisedAlertsForAlertDefinitionId(alarmDefinitionId);
    }
}
