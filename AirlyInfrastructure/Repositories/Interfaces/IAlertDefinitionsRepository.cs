using AirlyInfrastructure.Database;

namespace AirlyInfrastructure.Repositories.Interfaces
{
    public interface IAlertDefinitionsRepository
    {
        Task<AlertDefinition> AddAlertDefinitionAsync(AlertDefinition alertDefinition);
        Task<List<AlertDefinition>> GetAlertDefinitions();
    }
}
