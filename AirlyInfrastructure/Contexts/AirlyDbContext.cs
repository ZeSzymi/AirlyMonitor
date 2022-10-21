using AirlyInfrastructure.Database;
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
        public AirlyDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Installation>(installation =>
            {
                installation.ToTable("Installations");
                installation.HasKey(i => i.Id);
                installation.Ignore(i => i.Location);
            });


            modelBuilder.Entity<Measurement>(measurement =>
            {
                measurement.ToTable("Measurements");
                measurement.HasKey(m => m.InstallationId);
                measurement.Ignore(m => m.MeasurementValues);
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
            });
        }
    }
}
