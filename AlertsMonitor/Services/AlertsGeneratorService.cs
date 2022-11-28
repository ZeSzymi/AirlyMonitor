using AirlyInfrastructure.Database;
using AirlyInfrastructure.Repositories.Interfaces;
using AlertsMonitor.Services.Interfaces;

namespace AlertsMonitor.Services
{
    public class AlertsGeneratorService : IAlertsGeneratorService
    {
        private readonly IAlertsRepository _alertsRepository;
        private readonly ILogger<AlertsGeneratorService> _logger;

        public AlertsGeneratorService(IAlertsRepository alertsRepository, ILogger<AlertsGeneratorService> logger)
        {
            _alertsRepository = alertsRepository;
            _logger = logger;
        }

        public async Task<List<Alert>> AddAlertsAsync(List<AlertDefinition> alertDefinitions, List<Measurement> measurements, DateTime utcNow)
        {
            var alerts = new List<Alert>();

            foreach (var alertDefinition in alertDefinitions)
            {
                _logger.LogInformation($"Evaluating alertDefinition: {alertDefinition.Id} for installationId: {alertDefinition.InstallationId}");

                var measurement = measurements.FirstOrDefault(m => m.InstallationId == alertDefinition.InstallationId);
                if (measurement == null)
                {
                    continue;
                }

                var alertReports = alertDefinition.AlertRules
                    .Select(alertRule => alertRule.GetAlertReport(measurement.MeasurementValues.FirstOrDefault(mv => mv.Name == alertRule.MeasurementName)))
                    .Where(a => a != null)
                    .ToList();

                var alert = new Alert
                {
                    DateTime = utcNow,
                    AlertDefinitionId = alertDefinition.Id,
                    InstallationId = alertDefinition.InstallationId,
                    AlertReports = alertReports,
                    RaiseAlert = alertReports.Any(r => r.RaiseAlert)
                };

                _logger.LogInformation($"Alert report: {alert.Reports}");

                alerts.Add(alert);
            }

            return await _alertsRepository.AddAlertsAsync(alerts);
        }
    }
}
