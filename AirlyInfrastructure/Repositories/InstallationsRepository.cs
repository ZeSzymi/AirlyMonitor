using AirlyInfrastructure.Contexts;
using AirlyInfrastructure.Models.Database;
using AirlyInfrastructure.Repositories.Interfaces;
using AirlyMonitor.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace AirlyInfrastructure.Repositories
{
    public class InstallationsRepository : IInstallationsRepository
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

        public async Task<UserInstallation> AddUserInstallationAsync(UserInstallation userInstallation)
        {
            await _context.UserInstallations.AddAsync(userInstallation);
            await _context.SaveChangesAsync();
            return userInstallation;
        }

        public Task<UserInstallation?> GetUserInstallationAsync(string userId, int installationId)
            => _context.UserInstallations.FirstOrDefaultAsync(u => u.UserId == userId && u.InstallationId == installationId);

        public Task<List<Installation>> GetInstallationsForUserAsync(string userId)
            => _context.UserInstallations.Include(u => u.Installation).Where(u => u.UserId == userId).Select(u => u.Installation).ToListAsync();

        public Task<Installation?> GetInstallationAsync(int installationId) => _context
            .Installations
            .AsNoTracking()
            .Where(i => i.Id == installationId)
            .FirstOrDefaultAsync();
    }
}
