using AirlyInfrastructure.Database;
using AirlyInfrastructure.Repositories.Interfaces;
using AirlyInfrastructure.Services.Interfaces;

namespace AirlyInfrastructure.Services
{
    public class AlertDefinitionsService : IAlertDefinitionService
    {
        private readonly IAlertDefinitionsRepository _alertDefinitionsRepository;

        public AlertDefinitionsService(IAlertDefinitionsRepository alertDefinitionsRepository)
        {
            _alertDefinitionsRepository = alertDefinitionsRepository;
        }

        public Task<List<AlertDefinition>> GetAlertDefinitionsAsync() =>
            _alertDefinitionsRepository.GetAlertDefinitionsAsync();
    }
}
