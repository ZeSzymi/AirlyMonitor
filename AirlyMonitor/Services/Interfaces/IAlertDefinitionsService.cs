using AirlyInfrastructure.Database;

namespace AirlyMonitor.Services.Interfaces
{
    public interface IAlertDefinitionsService
    {
        Task<AlertDefinition> AddAlertDefinitionAsync(AlertDefinition alertDefinition);
    }
}
