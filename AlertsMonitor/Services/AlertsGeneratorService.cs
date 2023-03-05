using AirlyInfrastructure.Database;
using AirlyInfrastructure.Repositories.Interfaces;
using AirlyInfrastructure.Services.Interfaces;
using AlertsMonitor.Services.Interfaces;

namespace AlertsMonitor.Services
{
    public class AlertsGeneratorService : IAlertsGeneratorService
    {
        private readonly IAlertsRepository _alertsRepository;
        private readonly ILogger<AlertsGeneratorService> _logger;

        public AlertsGeneratorService(
            IAlertsRepository alertsRepository, 
            ILogger<AlertsGeneratorService> logger)
        {
            _alertsRepository = alertsRepository;
            _logger = logger;
        }

        public async Task<List<Alert>> AddAlertsAsync(List<AlertDefinition> alertDefinitions, List<Alert> previousAlerts, List<Measurement> measurements, DateTime utcNow)
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

                var previousAlert = previousAlerts.FirstOrDefault(a => a.AlertDefinitionId == alertDefinition.Id);
                Alert alert;

                if (alertDefinition.AQIThreshold != null)
                {
                    var alertReport = alertDefinition.GetAQIAlertReport(measurement.AQI);

                    alert = new Alert
                    {
                        DateTime = utcNow,
                        AlertDefinitionId = alertDefinition.Id,
                        InstallationId = alertDefinition.InstallationId,
                        AlertReports = null,
                        AQIAlertReport = alertReport,
                        PreviousRaisedAlert = previousAlert?.RaiseAlert ?? false,
                        RaiseAlert = alertReport.RaiseAlert
                    };

                } else
                {
                    var alertReports = alertDefinition.AlertRules
                   .Select(alertRule => alertRule.GetAlertReport(measurement.MeasurementValues.FirstOrDefault(mv => mv.Name == alertRule.MeasurementName)))
                   .Where(a => a != null)
                   .ToList();

                    alert = new Alert
                    {
                        DateTime = utcNow,
                        AlertDefinitionId = alertDefinition.Id,
                        InstallationId = alertDefinition.InstallationId,
                        AlertReports = alertReports,
                        PreviousRaisedAlert = previousAlert?.RaiseAlert ?? false,
                        RaiseAlert = alertReports.Any(r => r.RaiseAlert)
                    };
                }

                _logger.LogInformation($"Alert report: {alert.Reports} | AQI alert report: {alert.AQIReport}");

                alerts.Add(alert);
            }

            return await _alertsRepository.AddAlertsAsync(alerts);
        }
    }
}
