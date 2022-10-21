using AirlyInfrastructure.Database;
using AirlyMonitor.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace AirlyInfrastructure.Contexts
{
    public class AirlyDbContext : DbContext
    {
        public DbSet<Installation> Installations { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
        public AirlyDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Installation>(installation =>
            {
                installation.ToTable("Installations");
                installation.HasKey(i => i.Id);
            });


            modelBuilder.Entity<Measurement>(measurement =>
            {
                measurement.ToTable("Measurements");
                measurement.HasKey(i => i.InstallationId);
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
        }
    }
}
