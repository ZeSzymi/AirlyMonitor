using AirlyInfrastructure.Repositories.Interfaces;
using AirlyInfrastructure.Repositories;
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

                //if (_lastRun.AddMinutes(1) < DateTime.UtcNow)
                //{
                    _lastRun = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour, 0, 0);
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var measurementGenerationService = scope.ServiceProvider.GetRequiredService<IMeasurementGenerationService>();
                        var alertDefinitionsRepository = scope.ServiceProvider.GetRequiredService<IAlertDefinitionsRepository>();
                        var measurementRepository = scope.ServiceProvider.GetRequiredService<IMeasurementRepository>();
                        var logger = scope.ServiceProvider.GetRequiredService<ILogger<MeasurementsBackgroundService>>();

                        try
                        {
                            var installationIds = (await alertDefinitionsRepository.GetAlertDefinitionsAsync()).Select(a => a.InstallationId).ToList();
                            var measurements = measurementGenerationService.GenerateMeasurements(installationIds, utcNow);
                            await measurementRepository.AddMeasurementsAsync(measurements);
                    } catch (Exception e)
                        {
                            logger.LogError(e, e.Message);
                            Console.WriteLine(e.Message);
                        }

                    }
               // }
            }
        }
    }
}
