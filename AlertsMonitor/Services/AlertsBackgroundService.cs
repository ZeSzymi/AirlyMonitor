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
                    var alertsMonitorService = scope.ServiceProvider.GetRequiredService<IAlertsMonitorService>();
                    await alertsMonitorService.EvaluateAlerts(utcNow);
                }   
            }
        }
    }
}
