using AirlyInfrastructure.Database;

namespace AirlyInfrastructure.Services.Interfaces
{
    public interface IMeasurementGenerationService
    {
        List<Measurement> GenerateMeasurementsForInstrumentsInArea(List<int> installationIds);
        List<Measurement> GenerateMeasurements(List<int> installationIds, List<Measurement> measurements, DateTime utcNow);
        List<int> GetInstallationIdsToAddMeasurementTo(List<int> installationIds, List<Measurement> measurements, DateTime utcNow);
    }
}
