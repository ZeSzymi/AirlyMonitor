namespace AirlyInfrastructure.Models.Database
{
    public class AlertReport
    {
        public string MeasurementName { get; set; }
        public double? Min { get; set; }
        public double? Max { get; set; }
        public double? Exact { get; set; }
        public double? Actual { get; set; }
        public bool RaiseAlert { get; set; }
        public AlertResult Result { get; set; }
    }

    public enum AlertResult
    {
        Above,
        Below,
        Between,
        Exact
    }
}
