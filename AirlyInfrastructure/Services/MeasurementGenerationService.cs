using AirlyInfrastructure.Database;
using AirlyInfrastructure.Models.Constants;
using AirlyInfrastructure.Services.Interfaces;

namespace AirlyInfrastructure.Services
{
    public class MeasurementGenerationService : IMeasurementGenerationService
    {
        public List<int> GetInstallationIdsToAddMeasurementTo(List<int> installationIds, List<Measurement> measurements, DateTime utcNow)
        {
            var now = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour, 0, 0);
            var installationdsWithoutMeasurements = installationIds.Where(id => !measurements.Select(m => m.InstallationId).Contains(id)).ToList();
            return measurements.Where(m => m.TillDateTime.AddHours(1) <= now).Select(m => m.InstallationId).Concat(installationdsWithoutMeasurements).ToList();
        }

        public List<Measurement> GenerateMeasurementsForInstrumentsInArea(List<int> installationIds)
        {
            var utcNow = DateTime.UtcNow;
            var from = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour - 1, 0, 0);
            var till = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour, 0, 0);
            var measurement = new Measurement
            {
                InstallationId = 0,
                FromDateTime = from,
                TillDateTime = till,
                MeasurementValues = new List<MeasurementValue>
                    {
                        GenerateMeasurement(MeasurementValueNames.PM1, null, 85.55, 1.8, 10),
                        GenerateMeasurement(MeasurementValueNames.PM25, null, 161.33, 15.05, 15),
                        GenerateMeasurement(MeasurementValueNames.PM10, null, 250.10, 20.01, 15),
                        GenerateMeasurement(MeasurementValueNames.PRESSURE, null, 1300.63, 850.40, 100),
                        GenerateMeasurement(MeasurementValueNames.HUMIDITY, null, 110.63, 60.28, 10),
                        GenerateMeasurement(MeasurementValueNames.TEMPERATURE, null, 42.60, 0.05, 5),
                        GenerateMeasurement(MeasurementValueNames.NO2, null, 60.06, 10.05, 10),
                        GenerateMeasurement(MeasurementValueNames.O3, null, 35.50, 00.00, 5)
                    }
            };

            var measurements = installationIds.Select(id => new Measurement { InstallationId = id, FromDateTime = from, TillDateTime = till, MeasurementValues = measurement.MeasurementValues }).ToList();

            return GenerateMeasurements(installationIds, measurements, utcNow);
        }

        public List<Measurement> GenerateMeasurements(List<int> installationIds, List<Measurement> measurements, DateTime utcNow)
        {
            var from = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour - 1, 0, 0);
            var till = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour, 0, 0);

            return installationIds.Select(id =>
            {
                var measurement = measurements.FirstOrDefault(m => m.InstallationId == id)?.MeasurementValues;
                return new Measurement
                {
                    InstallationId = id,
                    FromDateTime = from,
                    TillDateTime = till,
                    MeasurementValues = new List<MeasurementValue>
                    {
                        GenerateMeasurement(MeasurementValueNames.PM1, measurement, 85.55, 1.8, 10),
                        GenerateMeasurement(MeasurementValueNames.PM25, measurement, 161.33, 15.05, 15),
                        GenerateMeasurement(MeasurementValueNames.PM10, measurement, 250.10, 20.01, 15),
                        GenerateMeasurement(MeasurementValueNames.PRESSURE, measurement, 1300.63, 850.40, 100),
                        GenerateMeasurement(MeasurementValueNames.HUMIDITY, measurement, 110.63, 60.28, 10),
                        GenerateMeasurement(MeasurementValueNames.TEMPERATURE, measurement, 42.60, 0.05, 5),
                        GenerateMeasurement(MeasurementValueNames.NO2, measurement, 60.06, 10.05, 10),
                        GenerateMeasurement(MeasurementValueNames.O3, measurement, 35.50, 00.00, 5)
                    }
                };
            }).ToList();
        }

        public MeasurementValue GenerateMeasurement(string name, List<MeasurementValue>? measurementValues, double maxValue, double minValue, double deviation)
        {
            Random random = new();
            var value = measurementValues?.First(v => v.Name == name).Value ?? random.NextDouble() * (maxValue - minValue) + minValue;

            if ((value - deviation) >= minValue)
            {
                minValue = (value - deviation);
            }

            if ((value + deviation) <= maxValue)
            {
                maxValue = (value + deviation);
            }

            return new MeasurementValue
            {
                Name = name,
                Value = random.NextDouble() * (maxValue - minValue) + minValue

            };
        }
    }
}
