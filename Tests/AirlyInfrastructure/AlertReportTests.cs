using AirlyInfrastructure.Models.Database;

namespace Tests.AirlyInfrastructure
{
    [TestFixture]
    public class AlertReportTests
    {
        [Test]
        public void ShouldReturnAQIAlertMessageWhenAQIThresholdIsSet()
        {
            var alertReport = new AlertReport
            {
                Actual = 120,
                AQIThreshold = 100
            };

            var reportMessage = alertReport.GetReportMessage();

            Assert.AreEqual("AQI is 120 and has crossed the threshold of 100", reportMessage);
        }

        [Test]
        public void ShouldReturnBetweenAlertMessageWhenResultIsBetween()
        {

            var alertReport = new AlertReport
            {
                MeasurementName = "Temperature",
                Actual = 25,
                Min = 20,
                Max = 30,
                Result = AlertResult.Between
            };

            var reportMessage = alertReport.GetReportMessage();

            Assert.AreEqual("Temperature has 25 measurement which does not fit between 20 and 30", reportMessage);
        }

        [Test]
        public void ShouldReturnBelowAlertMessageWhenResultIsBelow()
        {
            var alertReport = new AlertReport
            {
                MeasurementName = "Humidity",
                Actual = 10,
                Min = 20,
                Result = AlertResult.Below
            };

            var reportMessage = alertReport.GetReportMessage();

            Assert.AreEqual("Humidity has 10 measurement which does not fit below 20", reportMessage);
        }

        [Test]
        public void ShouldReturnAboveAlertMessageWhenResultIsAbove()
        {
            var alertReport = new AlertReport
            {
                MeasurementName = "Pressure",
                Actual = 1100,
                Max = 1000,
                Result = AlertResult.Above
            };

            var reportMessage = alertReport.GetReportMessage();

            Assert.AreEqual("Pressure has 1100 measurement which does not fit above 1000", reportMessage);
        }

        [Test]
        public void ShouldReturnExactAlertMessageWhenResultIsExact()
        {
            var alertReport = new AlertReport
            {
                MeasurementName = "CO2",
                Actual = 500,
                Exact = 400,
                Result = AlertResult.Exact
            };

            var reportMessage = alertReport.GetReportMessage();

            Assert.AreEqual("CO2 has 500 measurement which does not fit the 400 measurement", reportMessage);
        }
    }
}
