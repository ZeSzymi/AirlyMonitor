using AirlyInfrastructure.Database;

namespace MeasurementsSimulator.Services.Interfaces
{
    public interface IMeasurementGenerationService
    {
        List<Measurement> GenerateMeasurements(List<int> installationIds, DateTime utcNow);
    }
}
