using AirlyInfrastructure.Database;

namespace AirlyInfrastructure.Repositories.Interfaces
{
    public interface IAlertDefinitionsRepository
    {
        Task<AlertDefinition> AddAlertDefinitionAsync(AlertDefinition alertDefinition);
        Task<List<AlertDefinition>> GetAlertDefinitionsAsync();
        Task<List<AlertDefinition>> GetAlertDefinitionsAsync(int installationId);
        Task<List<AlertDefinition>> GetAlertDefinitionsForUserAsync(string userId);
        
    }
}
