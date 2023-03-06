using MeasurementsSimulator.Services.Interfaces;

namespace MeasurementsSimulator.Services
{
    public class MeasurementsBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public MeasurementsBackgroundService(IServiceScopeFactory serviceScopeFactory) => _serviceScopeFactory = serviceScopeFactory;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var timer = new PeriodicTimer(TimeSpan.FromMinutes(10));
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                var utcNow = DateTime.UtcNow;
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var measurementSimulatorService = scope.ServiceProvider.GetRequiredService<IMeasurementSimulatorService>();
                    await measurementSimulatorService.SimulateAsync(utcNow);
                }
            }
        }
    }
}
