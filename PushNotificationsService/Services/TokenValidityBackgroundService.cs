using PushNotificationsService.Services.Interfaces;

namespace PushNotificationsService.Services
{
    public class TokenValidityBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public TokenValidityBackgroundService(IServiceScopeFactory serviceScopeFactory) => _serviceScopeFactory = serviceScopeFactory;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var timer = new PeriodicTimer(TimeSpan.FromHours(12));
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var measurementSimulatorService = scope.ServiceProvider.GetRequiredService<IFirebaseCloudMessageService>();
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<TokenValidityBackgroundService>>();
                    logger.LogInformation("Checking device tokens validity");
                    await measurementSimulatorService.CheckDeviceTokenValidityAsync();
                }
            }
        }
    }
}
