using AirlyMonitor.Models.Database;

namespace AirlyMonitor.Models.Dtos
{
    public class InstallationDto
    {
        public InstallationDto(Installation installation)
        {
            Id = installation.Id;
            Address = installation.Address;
            Elevation = installation.Elevation;
            Sponsor = installation.Sponsor;
            Location = installation.Location;
        }

        public InstallationDto()
        {
        }

        public int Id { get; set; }
        public Address Address { get; set; }
        public double Elevation { get; set; }
        public Sponsor Sponsor { get; set; }
        public Location Location { get; set; }
        public double DistanceToInstallationMeters { get; set; }
    }
}
