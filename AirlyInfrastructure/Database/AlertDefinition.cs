namespace AirlyInfrastructure.Database
{
    public class AlertDefinition
    {
        public Guid? Id { get; set; }
        public int InstallationId { get; set; }
        public int CheckEvery { get; set; }
        public string Rules { get; set; }
        public bool? Deleted { get; set; }
    }
}
