using AirlyInfrastructure.Database;

namespace AlertsMonitor.Services.Interfaces
{
    public interface IAlertsGeneratorService
    {
        Task<List<Alert>> AddAlertsAsync(List<AlertDefinition> alertDefinitions, List<Alert> previousAlerts, List<Measurement> measurements, DateTime utcNow);
    }
}
