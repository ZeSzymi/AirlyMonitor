using MeasurementsSimulator.Services;

namespace Tests.MeasurementsSimulatorTests
{
    [TestFixture]
    public class MeasurementGenerationServiceTests
    {
        private MeasurementGenerationService _measurementGenerationService;

        [SetUp]
        public void Setup()
        {
            _measurementGenerationService = new MeasurementGenerationService();
        }

        [Test]
        public void GenerateMeasurements_ReturnsListWithSameCountAsInstallationIds()
        {
            var installationIds = new List<int> { 1, 2, 3 };
            var utcNow = DateTime.UtcNow;

            var measurements = _measurementGenerationService.GenerateMeasurements(installationIds, utcNow);

            Assert.AreEqual(installationIds.Count, measurements.Count);
        }

        [Test]
        public void GenerateMeasurements_ReturnsMeasurementsWithCorrectInstallationIds()
        {
            var installationIds = new List<int> { 1, 2, 3 };
            var utcNow = DateTime.UtcNow;

            var measurements = _measurementGenerationService.GenerateMeasurements(installationIds, utcNow);

            foreach (var measurement in measurements)
            {
                Assert.Contains(measurement.InstallationId, installationIds);
            }
        }

        [Test]
        public void GenerateMeasurements_ReturnsMeasurementsWithCorrectTimeRange()
        {
            var installationIds = new List<int> { 1 };
            var utcNow = DateTime.UtcNow;

            var measurements = _measurementGenerationService.GenerateMeasurements(installationIds, utcNow);

            foreach (var measurement in measurements)
            {
                var timeDiff = measurement.TillDateTime - measurement.FromDateTime;
                Assert.AreEqual(timeDiff.TotalHours, 1);
                Assert.AreEqual(measurement.FromDateTime.Minute, 0);
                Assert.AreEqual(measurement.FromDateTime.Second, 0);
                Assert.AreEqual(measurement.TillDateTime.Minute, 0);
                Assert.AreEqual(measurement.TillDateTime.Second, 0);
            }
        }

        [Test]
        public void GeneratePM1_ReturnsMeasurementValueInRange()
        {
            var measurementValue = _measurementGenerationService.GeneratePM1();

            Assert.IsTrue(measurementValue.Value >= 1.8 && measurementValue.Value <= 55.55);
        }

        [Test]
        public void GeneratePM25_ReturnsMeasurementValueInRange()
        {
            var measurementValue = _measurementGenerationService.GeneratePM25();

            Assert.IsTrue(measurementValue.Value >= 15.05 && measurementValue.Value <= 90.33);
        }

        [Test]
        public void GeneratePM10_ReturnsMeasurementValueInRange()
        {
            var measurementValue = _measurementGenerationService.GeneratePM10();

            Assert.IsTrue(measurementValue.Value >= 20.01 && measurementValue.Value <= 120.10);
        }

        [Test]
        public void GeneratePressure_ReturnsMeasurementValueInRange()
        {
            var measurementValue = _measurementGenerationService.GeneratePressure();

            Assert.IsTrue(measurementValue.Value >= 850.40 && measurementValue.Value <= 1300.63);
        }

        [Test]
        public void GenerateHumidity_ReturnsMeasurementValueInRange()
        {
            var measurementValue = _measurementGenerationService.GenerateHumidity();

            Assert.IsTrue(measurementValue.Value >= 60.28 && measurementValue.Value <= 110.63);
        }
    }
}
