using AirlyInfrastructure.Models.Database;

namespace AirlyInfrastructure.Database
{
    public class AlertRule
    {
        public string? MeasurementName { get; set; }
        public double? Min { get; set; }
        public double? Max { get; set; }
        public double? Exact { get; set; }

        public AlertReport? GetAlertReport(MeasurementValue? measurementValue)
        {
            if (measurementValue == null)
            {
                return null;
            }

            var alertReport = new AlertReport
            {
                Actual = measurementValue.Value,
                MeasurementName = measurementValue.Name,
                Min = Min,
                Max = Max,
                Exact = Exact,
                RaiseAlert = false,
                Result = AlertResult.Between
            };

            if (Exact != null && measurementValue.Value != Exact.Value)
            {
                alertReport.Result = measurementValue.Value > Exact.Value ? AlertResult.Above : AlertResult.Below;
                alertReport.RaiseAlert = true;
                return alertReport;
            } else if (Exact != null)
            {
                alertReport.Result = AlertResult.Exact;
                return alertReport;
            }

            if (Min != null && measurementValue.Value < Min.Value)
            {
                alertReport.Result = AlertResult.Below;
                alertReport.RaiseAlert = true;
            }

            if (Max != null && measurementValue.Value > Max.Value)
            {
                alertReport.Result = AlertResult.Above;
                alertReport.RaiseAlert = true;
            }

            return alertReport;
        }
    }
}
