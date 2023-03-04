using AirlyInfrastructure.Services.Interfaces;
using AlertsMonitor.Services.Interfaces;

namespace AlertsMonitor.Services
{
    public class AlertsMonitorService: IAlertsMonitorService
    {
        private readonly IAlertsService _alertsService;
        private readonly IMeasurementsService _measurementsService;
        private readonly IAlertsGeneratorService _alertsGeneratorService;
        private readonly IAlertDefinitionService _alertDefinitionService;
        private readonly IMessagesCreateorService _messagesCreateorService;
        private readonly ILogger<AlertsMonitorService> _logger;

        public AlertsMonitorService(
            IAlertsService alertsService, 
            IMeasurementsService measurementsService, 
            IAlertsGeneratorService alertsGeneratorService, 
            IAlertDefinitionService alertDefinitionService,
            IMessagesCreateorService messagesCreateorService,
            ILogger<AlertsMonitorService> logger
            )
        {
            _alertsService = alertsService;
            _measurementsService = measurementsService;
            _alertsGeneratorService = alertsGeneratorService;
            _alertDefinitionService = alertDefinitionService;
            _messagesCreateorService = messagesCreateorService;
            _logger = logger;
        }

        public async Task EvaluateAlerts(DateTime now)
        {
            try
            {
                _logger.LogInformation("Alert evaluation started");
                var alertDefinitions = await _alertDefinitionService.GetAlertDefinitionsAsync();
                var alertDefinitionIds = alertDefinitions.Select(a => a.Id).ToList();
                var alerts = await _alertsService.GetLatestAlertsAsync(alertDefinitionIds);

                var alertDefinitionsToEvaluate = alertDefinitions.Where(alertDefinition =>
                {
                    var alert = alerts.FirstOrDefault(a => a.AlertDefinitionId == alertDefinition.Id);
                    if (alert == null)
                    {
                        return true;
                    }

                    if (now > alert.DateTime.AddMinutes(alertDefinition.CheckEvery))
                    {
                        return true;
                    }
                    return false;
                }).ToList();

                var installationIds = alertDefinitionsToEvaluate.Select(ad => ad.InstallationId).ToList();
                var measurements = await _measurementsService.GetMeasurementsAsync(installationIds);
                var addedAlerts = await _alertsGeneratorService.AddAlertsAsync(alertDefinitionsToEvaluate, alerts, measurements, now);
                await _messagesCreateorService.SendMessages(addedAlerts, alertDefinitions);
                _logger.LogInformation("Alert evaluation finished");
            } catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
        }
    }
}
