using AirlyMonitor.Models.Database;

namespace AirlyInfrastructure.Models.Database
{
    public class UserInstallation
    {
        public string UserId { get; set; }
        public int InstallationId { get; set; }

        public Installation Installation { get; set; }
    }
}
