using AirlyInfrastructure.Services.Interfaces;
using AlertsMonitor.Services.Interfaces;

namespace AlertsMonitor.Services
{
    public class AlertsBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public AlertsBackgroundService(IServiceScopeFactory serviceScopeFactory) => _serviceScopeFactory = serviceScopeFactory;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                var utcNow = DateTime.UtcNow;

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var alertsService = scope.ServiceProvider.GetRequiredService<IAlertsService>();
                    var measurementsService = scope.ServiceProvider.GetRequiredService<IMeasurementsService>();
                    var alertsGeneratorService = scope.ServiceProvider.GetRequiredService<IAlertsGeneratorService>();
                    var alertDefinitionsService = scope.ServiceProvider.GetRequiredService<IAlertDefinitionService>();

                    var alertDefinitions = await alertDefinitionsService.GetAlertDefinitionsAsync();
                    var alerts = await alertsService.GetLatestAlertsAsync(alertDefinitions.Select(a => a.Id).ToList());
                    var alertDefinitionsToRun = alertDefinitions.Where(alertDefinition =>
                    {
                        var alert = alerts.FirstOrDefault(a => a.AlertDefinitionId == alertDefinition.Id);
                        if (alert == null)
                        {
                            return true;
                        }

                        if (utcNow > alert.DateTime.AddMinutes(alertDefinition.CheckEvery))
                        {
                            return true;
                        }

                        return false;
                    }).ToList();

                    var measurements = await measurementsService.GetMeasurementsAsync(alertDefinitionsToRun.Select(ad => ad.InstallationId).ToList());
                    await alertsGeneratorService.AddAlertsAsync(alertDefinitionsToRun, measurements, utcNow);
                }
            }
        }
    }
}
