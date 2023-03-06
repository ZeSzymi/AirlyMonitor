using AirlyInfrastructure.Database;
using AirlyInfrastructure.Models.Constants;
using Newtonsoft.Json;

namespace Tests.AirlyInfrastructure
{
    public class Tests
    {
        [TestFixture]
        public class MeasurementTests
        {
            [Test]
            public void ShouldReturnMaxAqi()
            {
                var measurement = new Measurement
                {
                    MeasurementValues = new List<MeasurementValue>
                    {
                        new MeasurementValue { Name = "PM25", Value = 20 },
                        new MeasurementValue { Name = "PM10", Value = 40 },
                        new MeasurementValue { Name = "PM1", Value = 30 },
                    }
                };

                var aqi = measurement.AQI;

                Assert.AreEqual(88.461538461538453d, aqi);
            }

            [Test]
            public void ShouldReturnCorrectAqiValue()
            {
                var measurement = new Measurement();
                var breakpoints = new[] { 12.0, 35.4, 55.4, 150.4, 250.4, 350.4, 500.4 };
                var aqiValues = new double[] { 0, 50, 100, 150, 200, 300, 500 };
                var expectedAqi = 38.46153846153846d;

                var aqi = measurement.CalculateAqi(30, breakpoints, aqiValues);

                Assert.AreEqual(expectedAqi, aqi);
            }

            [Test]
            public void ShouldReturnMaximumAqi()
            {
                var measurement = new Measurement
                {
                    MeasurementValues = new List<MeasurementValue>
                    {
                        new MeasurementValue { Name = MeasurementValueNames.PM25, Value = 40 },
                        new MeasurementValue { Name = MeasurementValueNames.PM10, Value = 80 },
                        new MeasurementValue { Name = MeasurementValueNames.PM1, Value = 20 }
                    }
                };

                var result = measurement.AQI;

                Assert.AreEqual(67.09401709401709d, result);
            }

            [Test]
            public void ShouldDeserializeMeasurementValues()
            {
                var measurement = new Measurement
                {
                    Values = "[{\"Name\":\"PM25\",\"Value\":20.0},{\"Name\":\"PM10\",\"Value\":30.0}]"
                };

                var result = measurement.MeasurementValues;

                Assert.AreEqual(2, result.Count);
                Assert.AreEqual("PM25", result[0].Name);
                Assert.AreEqual(20.0, result[0].Value);
                Assert.AreEqual("PM10", result[1].Name);
                Assert.AreEqual(30.0, result[1].Value);
            }

            [Test]
            public void ShouldSerializeMeasurementValues()
            {
                var measurement = new Measurement
                {
                    MeasurementValues = new List<MeasurementValue>
                    {
                        new MeasurementValue { Name = MeasurementValueNames.PM25, Value = 20.0 },
                        new MeasurementValue { Name = MeasurementValueNames.PM10, Value = 30.0 }
                    }
                };

                var result = measurement.Values;

                Assert.AreEqual("[{\"Name\":\"PM25\",\"Value\":20.0},{\"Name\":\"PM10\",\"Value\":30.0}]", result);
            }

            [Test]
            public void ShouldCalculateAqiForPm25()
            {
                var measurement = new Measurement();
                var pm25 = 50.0;

                var result = measurement.GetAqiPM25(pm25);

                Assert.AreEqual(86.5d, result);
            }

            [Test]
            public void ShouldCalculateAqiForPm10()
            {
                var measurement = new Measurement();
                var pm10 = 100.0;

                var result = measurement.GetAqiPM10(pm10);

                Assert.AreEqual(23.0d, result);
            }

            [Test]
            public void ShouldCalculateAqiForPm1()
            {
                var measurement = new Measurement();
                var pm1 = 10.0;

                var result = measurement.GetAqiPM1(pm1);

                Assert.AreEqual(41.666666666666671d, result);
            }
        }
    }
}