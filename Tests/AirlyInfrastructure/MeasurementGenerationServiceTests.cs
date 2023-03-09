using AirlyInfrastructure.Database;
using AirlyInfrastructure.Models.Constants;
using AirlyInfrastructure.Services;

namespace Tests.AirlyInfrastructure
{
    [TestFixture]
    public class MeasurementGenerationServiceTests
    {
        [Test]
        public void GetInstallationIdsToAddMeasurementTo_ReturnsCorrectInstallationIds()
        {
            var service = new MeasurementGenerationService();
            var installationIds = new List<int> { 1, 2, 3 };
            var measurements = new List<Measurement>
            {
                new Measurement { InstallationId = 1, TillDateTime = DateTime.UtcNow.AddHours(-1) },
                new Measurement { InstallationId = 3, TillDateTime = DateTime.UtcNow.AddHours(-2) }
            };
            var utcNow = DateTime.UtcNow;

            var result = service.GetInstallationIdsToAddMeasurementTo(installationIds, measurements, utcNow);

            CollectionAssert.AreEquivalent(new List<int> { 2, 3 }, result);
        }

        [Test]
        public void GenerateMeasurementsForInstrumentsInArea_ReturnsMeasurementsWithCorrectInstallationIds()
        {
            var service = new MeasurementGenerationService();
            var installationIds = new List<int> { 1, 2, 3 };

            var result = service.GenerateMeasurementsForInstrumentsInArea(installationIds);

            Assert.AreEqual(installationIds.Count, result.Count);
            foreach (var measurement in result)
            {
                Assert.IsTrue(installationIds.Contains(measurement.InstallationId));
            }
        }

        [Test]
        public void GenerateMeasurements_ReturnsMeasurementsWithCorrectInstallationIds()
        {
            var installationIds = new List<int> { 1, 2 };
            var measurements = new List<Measurement>();
            var utcNow = DateTime.UtcNow;
            var service = new MeasurementGenerationService();

            var result = service.GenerateMeasurements(installationIds, measurements, utcNow);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].InstallationId, Is.EqualTo(1));
            Assert.That(result[1].InstallationId, Is.EqualTo(2));
        }

        [Test]
        public void GenerateMeasurement_ReturnsMeasurementWithCorrectNameAndValue()
        {
            var service = new MeasurementGenerationService();
            var name = MeasurementValueNames.PM1;
            var measurementValues = new List<MeasurementValue> { new MeasurementValue { Name = name, Value = 10 } };
            var maxValue = 20;
            var minValue = 5;
            var deviation = 3;

            var result = service.GenerateMeasurement(name, measurementValues, maxValue, minValue, deviation);

            Assert.AreEqual(name, result.Name);
            Assert.IsTrue(result.Value >= (measurementValues[0].Value - deviation));
            Assert.IsTrue(result.Value <= (measurementValues[0].Value + deviation));
            Assert.IsTrue(result.Value >= minValue);
            Assert.IsTrue(result.Value <= maxValue);
        }

        [Test]
        public void GenerateMeasurementsForInstrumentsInArea_EmptyInstallationIds_ReturnsEmptyList()
        {
            var service = new MeasurementGenerationService();
            var installationIds = new List<int>();

            var result = service.GenerateMeasurementsForInstrumentsInArea(installationIds);

            Assert.IsEmpty(result);
        }

        [Test]
        public void GenerateMeasurement_NullMeasurementValues_GeneratesRandomValue()
        {
            var service = new MeasurementGenerationService();
            List<MeasurementValue>? measurementValues = null;

            var result = service.GenerateMeasurement(MeasurementValueNames.PM1, measurementValues, 85.55, 1.8, 10);

            Assert.That(result.Value, Is.InRange(1.8, 85.55));
        }

        [Test]
        public void GetInstallationIdsToAddMeasurementTo_AllInstallationsAlreadyHaveMeasurements_ReturnsEmptyList()
        {
            var service = new MeasurementGenerationService();
            var installationIds = new List<int> { 1, 2, 3 };
            var measurements = new List<Measurement>
            {
                new Measurement { InstallationId = 1, FromDateTime = DateTime.Now.AddHours(-1), TillDateTime = DateTime.Now, MeasurementValues = new List<MeasurementValue>() },
                new Measurement { InstallationId = 2, FromDateTime = DateTime.Now.AddHours(-1), TillDateTime = DateTime.Now, MeasurementValues = new List<MeasurementValue>() },
                new Measurement { InstallationId = 3, FromDateTime = DateTime.Now.AddHours(-1), TillDateTime = DateTime.Now, MeasurementValues = new List<MeasurementValue>() }
            };
            var utcNow = DateTime.UtcNow;

            var result = service.GetInstallationIdsToAddMeasurementTo(installationIds, measurements, utcNow);

            Assert.IsEmpty(result);
        }

        [Test]
        public void GetInstallationIdsToAddMeasurementTo_AllInstallationsDoNotHaveMeasurements_ReturnsAllInstallations()
        {
            var service = new MeasurementGenerationService();
            var installationIds = new List<int> { 1, 2, 3 };
            var measurements = new List<Measurement>();
            var utcNow = DateTime.UtcNow;

            var result = service.GetInstallationIdsToAddMeasurementTo(installationIds, measurements, utcNow);

            Assert.That(result, Is.EqualTo(installationIds));
        }
    }
}