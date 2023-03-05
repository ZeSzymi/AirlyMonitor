using AirlyInfrastructure.Database;
using AirlyInfrastructure.Models.Database;
using AirlyMonitor.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace AirlyInfrastructure.Contexts
{
    public class AirlyDbContext : DbContext
    {
        public DbSet<Installation> Installations { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Sponsor> Sponsors { get; set; }
        public DbSet<AlertDefinition> AlertDefinitions { get; set; }
        public DbSet<Alert> Alerts { get; set; }
        public DbSet<UserInstallation> UserInstallations { get; set; }
        public DbSet<User> Users { get; set; }
        public AirlyDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Installation>(installation =>
            {
                installation.ToTable("Installations");
                installation.HasKey(i => i.Id);
                installation.Ignore(i => i.Location);
                installation.HasOne(i => i.Address).WithOne();
                installation.HasOne(i => i.Sponsor).WithOne();
            });


            modelBuilder.Entity<Measurement>(measurement =>
            {
                measurement.ToTable("Measurements");
                measurement.HasKey(m => m.Id);
                measurement.Ignore(m => m.MeasurementValues);
                measurement.Ignore(m => m.AQI);
            });

            modelBuilder.Entity<Address>(measurement =>
            {
                measurement.ToTable("Addresses");
                measurement.HasKey(i => i.AddressId);
            });

            modelBuilder.Entity<Sponsor>(measurement =>
            {
                measurement.ToTable("Sponsors");
                measurement.HasKey(i => i.SponsorId);
            });

            modelBuilder.Entity<AlertDefinition>(alertDefinition =>
            {
                alertDefinition.ToTable("AlertDefinitions");
                alertDefinition.HasKey(i => i.Id);
                alertDefinition.Ignore(a => a.AlertRules);
            });

            modelBuilder.Entity<Alert>(alert =>
            {
                alert.ToTable("Alerts");
                alert.HasKey(i => i.Id);
                alert.Ignore(a => a.AlertReports);
                alert.Ignore(a => a.AQIAlertReport);
            });

            modelBuilder.Entity<User>(user =>
            {
                user.ToTable("AspNetUsers");
                user.HasKey(i => i.Id);
            });

            modelBuilder.Entity<UserInstallation>(userInstallation =>
            {
                userInstallation.ToTable("AspNetUsersInstallations");
                userInstallation.HasKey(u => new { u.UserId, u.InstallationId });

                userInstallation.HasOne(u => u.Installation).WithMany();
            });
        }
    }
}
