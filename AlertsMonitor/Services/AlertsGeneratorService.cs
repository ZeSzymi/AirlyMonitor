using AirlyInfrastructure.Database;
using AirlyInfrastructure.Repositories.Interfaces;
using AlertsMonitor.Services.Interfaces;

namespace AlertsMonitor.Services
{
    public class AlertsGeneratorService : IAlertsGeneratorService
    {
        private readonly IAlertsRepository _alertsRepository;

        public AlertsGeneratorService(IAlertsRepository alertsRepository)
        {
            _alertsRepository = alertsRepository;
        }

        public async Task<List<Alert>> AddAlertsAsync(List<AlertDefinition> alertDefinitions, List<Measurement> measurements, DateTime utcNow)
        {
            var alerts = new List<Alert>();

            foreach (var alertDefinition in alertDefinitions)
            {
                var measurement = measurements.FirstOrDefault(m => m.InstallationId == alertDefinition.InstallationId);
                if (measurement == null)
                {
                    continue;
                }

                var alertReports = alertDefinition.AlertRules
                    .Select(alertRule => alertRule.GetAlertReport(measurement.MeasurementValues.FirstOrDefault(mv => mv.Name == alertRule.MeasurementName)))
                    .Where(a => a != null)
                    .ToList();

                alerts.Add(new Alert
                {
                    DateTime = utcNow,
                    AlertDefinitionId = alertDefinition.Id,
                    InstallationId = alertDefinition.InstallationId,
                    AlertReports = alertReports,
                    RaiseAlert = alertReports.Any(r => r.RaiseAlert)
                });
            }

            return await _alertsRepository.AddAlertsAsync(alerts);
        }
    }
}
