using AirlyInfrastructure.Models.Database;
using Newtonsoft.Json;

namespace AirlyInfrastructure.Database
{
    public class AlertDefinition
    {
        public string UserId { get; set; }
        public Guid Id { get; set; }
        public int InstallationId { get; set; }
        public int CheckEvery { get; set; }
        public string Rules { get; set; }
        public bool Deleted { get; set; }
        public double? AQIThreshold { get; set; }
        public bool Active { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

        public List<AlertRule> AlertRules
        {
            get => JsonConvert.DeserializeObject<List<AlertRule>>(Rules);
            set => Rules = JsonConvert.SerializeObject(value ?? new List<AlertRule>());
        }

        public AlertReport GetAQIAlertReport(double aqi)
        {
            if (aqi > AQIThreshold)
            {
                return new AlertReport
                {
                    MeasurementName = "AQI",
                    Actual = aqi,
                    AQIThreshold = AQIThreshold,
                    RaiseAlert = true
                };
            }

            return new AlertReport
            {
                MeasurementName = "AQI",
                AQIThreshold = AQIThreshold,
                Actual = aqi,
                RaiseAlert = false
            };
        }
    }
}
