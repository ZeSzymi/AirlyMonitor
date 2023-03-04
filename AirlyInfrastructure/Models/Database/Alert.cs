using AirlyInfrastructure.Models.Database;
using Newtonsoft.Json;

namespace AirlyInfrastructure.Database
{
    public class Alert
    {
        public Guid Id { get; set; }
        public Guid AlertDefinitionId { get; set; }
        public int InstallationId { get; set; }
        public DateTime DateTime { get; set; }
        public string Reports { get; set; }
        public bool RaiseAlert { get; set; }
        public bool PreviousRaisedAlert { get; set; }
        public string AQIReport { get; set; }

        public List<AlertReport> AlertReports
        {
            get => JsonConvert.DeserializeObject<List<AlertReport>>(Reports);
            set => Reports = JsonConvert.SerializeObject(value ?? new List<AlertReport>());
        }

        public AlertReport AQIAlertReports
        {
            get => JsonConvert.DeserializeObject<AlertReport>(Reports);
            set => AQIReport = JsonConvert.SerializeObject(value ?? new AlertReport());
        }
    }
}
