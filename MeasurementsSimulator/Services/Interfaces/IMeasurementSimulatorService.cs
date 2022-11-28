namespace MeasurementsSimulator.Services.Interfaces
{
    public interface IMeasurementSimulatorService
    {
        Task SimulateAsync(DateTime now);
    }
}
