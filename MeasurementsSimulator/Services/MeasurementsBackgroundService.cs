using MeasurementsSimulator.Services.Interfaces;

namespace MeasurementsSimulator.Services
{
    public class MeasurementsBackgroundService : BackgroundService
    {
        private DateTime _lastRun  = DateTime.MinValue;

        private readonly IServiceScopeFactory _serviceScopeFactory;
        public MeasurementsBackgroundService(IServiceScopeFactory serviceScopeFactory) => _serviceScopeFactory = serviceScopeFactory;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                var utcNow = DateTime.UtcNow;

                if (_lastRun.AddHours(1) < DateTime.UtcNow)
                {
                    _lastRun = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour, 0, 0);
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var measurementService = scope.ServiceProvider.GetRequiredService<IMeasurementService>();
                        await measurementService.AddMeasurementsAsync(_lastRun);
                    }
                }
            }
        }
    }
}
