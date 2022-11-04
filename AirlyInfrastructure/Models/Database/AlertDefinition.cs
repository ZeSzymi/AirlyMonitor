using Newtonsoft.Json;

namespace AirlyInfrastructure.Database
{
    public class AlertDefinition
    {
        public Guid Id { get; set; }
        public int InstallationId { get; set; }
        public int CheckEvery { get; set; }
        public string Rules { get; set; }
        public bool Deleted { get; set; }
        public bool Active { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

        public List<AlertRule> AlertRules
        {
            get => JsonConvert.DeserializeObject<List<AlertRule>>(Rules);
            set => Rules = JsonConvert.SerializeObject(value);
        }
    }
}
