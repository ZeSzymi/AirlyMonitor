using AirlyInfrastructure.Contexts;
using AirlyInfrastructure.Repositories.Interfaces;
using AirlyMonitor.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace AirlyInfrastructure.Repositories
{
    public class InstallationsRepository : IInstallationRepository
    {
        private readonly AirlyDbContext _context;

        public InstallationsRepository(AirlyDbContext context)
        {
            _context = context;
        }

        public Task<List<int>> GetInstallationIdsAsync() => _context
            .Installations
            .AsNoTracking()
            .Where(i => i.Deleted == false)
            .Select(i => i.Id)
            .ToListAsync();

        public async Task<Installation> AddInstallationAsync(Installation installation)
        {
            var address = installation.Address;
            var sponsor = installation.Sponsor;
            var location = installation.Location;
            installation.Latitude = location.Latitude;
            installation.Longitude = location.Longitude;
            await _context.Addresses.AddAsync(address);
            await _context.Sponsors.AddAsync(sponsor);
            await _context.Installations.AddAsync(installation);
            await _context.SaveChangesAsync();
            return installation;
        }

        public Task<Installation?> GetInstallationAsync(int installationId) => _context
            .Installations
            .AsNoTracking()
            .Where(i => i.Id == installationId)
            .FirstOrDefaultAsync();
    }
}
