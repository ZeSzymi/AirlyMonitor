namespace AirlyInfrastructure.Database
{
    public class Alert
    {
        public Guid Id { get; set; }
        public Guid AlertDefinitionId { get; set; }
        public int InstallationId { get; set; }
        public DateTime DateTime { get; set; }
        public string Details { get; set; }
    }
}
