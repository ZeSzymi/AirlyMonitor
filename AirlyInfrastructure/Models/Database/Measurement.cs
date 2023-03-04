using AirlyInfrastructure.Models.Constants;
using Newtonsoft.Json;

namespace AirlyInfrastructure.Database
{
    public class Measurement
    {
        public Guid Id { get; set; }
        public int InstallationId { get; set; }
        public DateTime FromDateTime { get; set; }
        public DateTime TillDateTime { get; set; }
        public string Values { get; set; }
        public double AQI
        {
            get
            {
                var measurementValues = MeasurementValues;
                var pm25 = measurementValues.First(m => m.Name == MeasurementValueNames.PM25).Value;
                var pm10 = measurementValues.First(m => m.Name == MeasurementValueNames.PM10).Value;
                var pm1 = measurementValues.First(m => m.Name == MeasurementValueNames.PM1).Value;

                var aqiPm25 = GetAqiPM25(pm25);
                var aqiPm10 = GetAqiPM10(pm10);
                var aqiPm1 = GetAqiPM1(pm1);

                return Math.Max(aqiPm25, Math.Max(aqiPm10, aqiPm1));
            }
        }

        public List<MeasurementValue> MeasurementValues
        {
            get => JsonConvert.DeserializeObject<List<MeasurementValue>>(Values);
            set => Values = JsonConvert.SerializeObject(value);
        }

        private double GetAqiPM25(double pm25)
        {
            var breakpoints = new[] { 12.0, 35.4, 55.4, 150.4, 250.4, 350.4, 500.4 };
            var aqiValues = new double[] { 0, 50, 100, 150, 200, 300, 500 };

            return CalculateAqi(pm25, breakpoints, aqiValues);
        }

        private double GetAqiPM10(double pm10)
        {
            var breakpoints = new[] { 54.0, 154.0, 254.0, 354.0, 424.0, 504.0, 604.0 };
            var aqiValues = new double[] { 0, 50, 100, 150, 200, 300, 500 };

            return CalculateAqi(pm10, breakpoints, aqiValues);
        }

        private double GetAqiPM1(double pm1)
        {
            var breakpoints = new[] { 0.0, 12.0, 35.4, 55.4, 150.4, 250.4, 500.4 };
            var aqiValues = new double[] { 0, 50, 100, 150, 200, 300, 500 };

            return CalculateAqi(pm1, breakpoints, aqiValues);
        }

        private double CalculateAqi(double measurement, double[] breakpoints, double[] aqiValues)
        {
            var iHigh = breakpoints.Length - 1;

            for (var i = 0; i < breakpoints.Length; i++)
            {
                if (measurement <= breakpoints[i])
                {
                    iHigh = i;
                    break;
                }
            }

            if (iHigh == 0)
            {
                return aqiValues[0];
            }
            else if (iHigh == breakpoints.Length - 1)
            {
                return aqiValues[iHigh];
            }
            else
            {
                var aqiLow = aqiValues[iHigh - 1];
                var aqiHigh = aqiValues[iHigh];
                var bpLow = breakpoints[iHigh - 1];
                var bpHigh = breakpoints[iHigh];

                return ((aqiHigh - aqiLow) / (bpHigh - bpLow)) * (measurement - bpLow) + aqiLow;
            }
        }
    }

    public class MeasurementValue
    {
        public string Name { get; set; }
        public double Value { get; set; }
    }
}
