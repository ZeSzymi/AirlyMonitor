using AirlyInfrastructure.Database;
using AirlyMonitor.Models.Database;

namespace AirlyMonitor.Models.Dtos
{
    public class InstallationDto
    {
        public InstallationDto(Installation installation, List<Measurement>? measurements = null)
        {
            var measurement = measurements?.FirstOrDefault(m => m.InstallationId == installation.Id);
            Id = installation.Id;
            Address = installation.Address;
            Elevation = installation.Elevation;
            Sponsor = installation.Sponsor;
            Location = installation.Location;
            if (measurement != null)
            {
                Measurement = new MeasurementDto(measurement);
            }
        }

        public InstallationDto()
        {
        }

        public int Id { get; set; }
        public Address Address { get; set; }
        public double Elevation { get; set; }
        public Sponsor Sponsor { get; set; }
        public Location Location { get; set; }
        public double? DistanceToInstallationMeters { get; set; }
        public bool Marked { get; set; }
        public bool HasAlert { get; set; }
        public MeasurementDto? Measurement { get; set; }
    }
}
