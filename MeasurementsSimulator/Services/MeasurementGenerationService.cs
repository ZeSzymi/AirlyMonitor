using AirlyInfrastructure.Database;
using AirlyInfrastructure.Models.Constants;
using MeasurementsSimulator.Services.Interfaces;

namespace MeasurementsSimulator.Services
{
    public class MeasurementGenerationService : IMeasurementGenerationService
    {
        public List<Measurement> GenerateMeasurements(List<int> installationIds, DateTime utcNow)
        {
            var from = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour - 1, 0, 0);
            var till = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour, 0, 0);

            return installationIds.Select(id => new Measurement
            {
                InstallationId = id,
                FromDateTime = from,
                TillDateTime = till,
                MeasurementValues = new List<MeasurementValue>
                {
                    GeneratePM1(),
                    GeneratePM25(),
                    GeneratePM10(),
                    GeneratePressure(),
                    GenerateHumidity(),
                    GenerateTemperature(),
                    GenerateNO2(),
                    GenerateO3()
                }
            }).ToList();
        }

        public MeasurementValue GeneratePM1()
        {
            Random random = new Random();
            return new MeasurementValue
            {
                Name = MeasurementValueNames.PM1,
                Value = random.NextDouble() * (55.55 - 1.8) + 1.8

            };
        }

        public MeasurementValue GeneratePM25()
        {
            Random random = new Random();
            return new MeasurementValue
            {
                Name = MeasurementValueNames.PM25,
                Value = random.NextDouble() * (90.33 - 15.05) + 15.05

            };
        }

        public MeasurementValue GeneratePM10()
        {
            Random random = new Random();
            return new MeasurementValue
            {
                Name = MeasurementValueNames.PM10,
                Value = random.NextDouble() * (120.10 - 20.01) + 20.01

            };
        }

        public MeasurementValue GeneratePressure()
        {
            Random random = new Random();
            return new MeasurementValue
            {
                Name = MeasurementValueNames.PRESSURE,
                Value = random.NextDouble() * (1300.63 - 850.40) + 850.40

            };
        }

        public MeasurementValue GenerateHumidity()
        {
            Random random = new Random();
            return new MeasurementValue
            {
                Name = MeasurementValueNames.HUMIDITY,
                Value = random.NextDouble() * (110.63 - 60.28) + 60.28

            };
        }

        public MeasurementValue GenerateTemperature()
        {
            Random random = new Random();
            return new MeasurementValue
            {
                Name = MeasurementValueNames.TEMPERATURE,
                Value = random.NextDouble() * (36.60 - 0.05) + 0.05

            };
        }

        public MeasurementValue GenerateNO2()
        {
            Random random = new Random();
            return new MeasurementValue
            {
                Name = MeasurementValueNames.NO2,
                Value = random.NextDouble() * (60.06 - 10.05) + 10.05

            };
        }

        public MeasurementValue GenerateO3()
        {
            Random random = new Random();
            return new MeasurementValue
            {
                Name = MeasurementValueNames.O3,
                Value = random.NextDouble() * (35.50 - 00.00) + 00.00

            };
        }
    }
}
