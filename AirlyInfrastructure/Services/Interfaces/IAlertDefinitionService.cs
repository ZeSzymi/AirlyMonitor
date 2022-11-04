using AirlyInfrastructure.Database;

namespace AirlyInfrastructure.Services.Interfaces
{
    public interface IAlertDefinitionService
    {
        Task<List<AlertDefinition>> GetAlertDefinitionsAsync();
    }
}
