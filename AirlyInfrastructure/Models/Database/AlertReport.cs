namespace AirlyInfrastructure.Models.Database
{
    public class AlertReport
    {
        public string MeasurementName { get; set; }
        public double? Min { get; set; }
        public double? Max { get; set; }
        public double? Exact { get; set; }
        public double? Actual { get; set; }
        public double? AQIThreshold { get; set; }
        public bool RaiseAlert { get; set; }
        public AlertResult Result { get; set; }

        public string GetReportMessage()
        {
            if (AQIThreshold != null)
            {
                return $"AQI is {Actual} and has crossed the threshold of {AQIThreshold}";
            }

            var message = $"{MeasurementName} has {Actual} measurement ";

            if (Result == AlertResult.Between)
            {
                message += $"which does not fit between {Min} and {Max}";
                return message;
            }

            if (Result == AlertResult.Below)
            {
                message += $"which does not fit below {Min}";
                return message;
            }

            if (Result == AlertResult.Above)
            {
                message += $"which does not fit above {Max}";
                return message;
            }

            if (Result == AlertResult.Exact)
            {
                message += $"which does not fit the {Exact} measurement";
                return message;
            }

            return message;
        }
    }

    public enum AlertResult
    {
        Above,
        Below,
        Between,
        Exact,
        AQI
    }
}
