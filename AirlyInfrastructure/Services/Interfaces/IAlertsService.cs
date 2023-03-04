using AirlyInfrastructure.Database;

namespace AirlyInfrastructure.Services.Interfaces
{
    public interface IAlertsService
    {
        Task<List<Alert>> GetLatestAlertsAsync(List<Guid> alertDefinitionIds);
        Task<List<Alert>> GetAlertsForAlertDefinitionId(Guid alarmDefinitionId);
        Task<List<Alert>> GetRaisedAlertsForAlertDefinitionId(Guid alarmDefinitionId);
    }
}
