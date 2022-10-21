using AirlyInfrastructure.Database;

namespace MeasurementsSimulator.Services.Interfaces
{
    public interface IMeasurementService
    {
        Task<List<Measurement>> AddMeasurementsAsync(DateTime utcNow);
    }
}
