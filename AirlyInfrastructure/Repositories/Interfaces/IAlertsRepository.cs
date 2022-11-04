using AirlyInfrastructure.Database;

namespace AirlyInfrastructure.Repositories.Interfaces
{
    public interface IAlertsRepository
    {
        Task<List<Alert>> AddAlertsAsync(List<Alert> alerts);
        Task<List<Alert>> GetAlertsAsync();
        Task<List<Alert>> GetLatestAlertsAsync(List<Guid> alarmDefinitionIds);
        Task<List<Alert>> GetAlertsForAlertDefinitionId(Guid alarmDefinitionId);
    }
}
