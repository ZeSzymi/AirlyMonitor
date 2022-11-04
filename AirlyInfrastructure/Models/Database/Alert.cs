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

        public List<AlertReport> AlertReports
        {
            get => JsonConvert.DeserializeObject<List<AlertReport>>(Reports);
            set => Reports = JsonConvert.SerializeObject(value);
        }
    }
}
