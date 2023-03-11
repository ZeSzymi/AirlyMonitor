using AirlyInfrastructure.Database;

namespace AirlyMonitor.Models.Dtos
{
    public class MeasurementDto
    {
        public MeasurementDto(Measurement measurement)
        {
            Id = measurement.Id;
            InstallationId = measurement.InstallationId;
            FromDateTime = measurement.FromDateTime;
            TillDateTime = measurement.TillDateTime;
            MeasurementValues = measurement.MeasurementValues;
            AQI = measurement.AQI;
        }

        public Guid Id { get; set; }
        public int InstallationId { get; set; }
        public DateTime FromDateTime { get; set; }
        public DateTime TillDateTime { get; set; }
        public double AQI { get; set; }
        public List<MeasurementValue> MeasurementValues {  get; set; }
    }
}
