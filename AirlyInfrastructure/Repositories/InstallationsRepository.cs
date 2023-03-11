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

        public async Task<List<Installation>> AddInstallationsAsync(List<Installation> installations)
        {

            var addresses = installations.Select(i => i.Address);
            var sponsors = installations.Select(i => i.Sponsor);
            var locations = installations.Select(i => i.Location);
            var installationIds = installations.Select(i => i.Id);
            var existingInstallations = await _context.Installations.Select(i => i.Id).Where(id => installationIds.Contains(id)).ToListAsync();
            var installationsToAdd = installations.Where(installation => !existingInstallations.Contains(installation.Id)).ToList();

            installationsToAdd.ForEach(installation =>
            {
                installation.Latitude = installation.Location.Latitude;
                installation.Longitude = installation.Location.Longitude;
            });

            await _context.Addresses.AddRangeAsync(addresses);
            await _context.Sponsors.AddRangeAsync(sponsors);
            await _context.Installations.AddRangeAsync(installationsToAdd);
            await _context.SaveChangesAsync();
            return installations;
        }

        public async Task<UserInstallation> AddUserInstallationAsync(UserInstallation userInstallation)
        {
            await _context.UserInstallations.AddAsync(userInstallation);
            await _context.SaveChangesAsync();
            return userInstallation;
        }

        public async Task RemoveUserInstallation(string userId, int installationId)
        {
            var userInstallation = _context.UserInstallations.Where(i => i.InstallationId == installationId && i.UserId == userId).FirstOrDefault();

            if (userInstallation == null)
            {
                return;
            }

            _context.UserInstallations.Remove(userInstallation);
            await _context.SaveChangesAsync();
        }

        public Task<UserInstallation?> GetUserInstallationAsync(string userId, int installationId)
            => _context.UserInstallations.FirstOrDefaultAsync(u => u.UserId == userId && u.InstallationId == installationId);

        public Task<List<Installation>> GetInstallationsForUserAsync(string userId)
            => _context.UserInstallations
            .Include(u => u.Installation)
            .ThenInclude(i => i.Sponsor)
            .Include(u => u.Installation)
            .ThenInclude(i => i.Address)
            .Where(u => u.UserId == userId)
            .Select(u => u.Installation)
            .ToListAsync();

        public Task<List<int>> GetUserInstallationIds(string userId, List<int> installationIds)
            => _context.UserInstallations
                .Where(ui => installationIds.Contains(ui.InstallationId) && ui.UserId == userId)
                .Select(ui => ui.InstallationId)
                .ToListAsync();

        public Task<List<Installation>> GetInstallationsAsync(List<int> installationIds)
           => _context.Installations
            .Include(installation => installation.Address)
            .Where(installation => installationIds.Contains(installation.Id))
            .ToListAsync();

        public Task<Installation?> GetInstallationAsync(int installationId) => _context
            .Installations
            .AsNoTracking()
            .Where(i => i.Id == installationId)
            .FirstOrDefaultAsync();

        public Task<List<int>> GetInstallationsIdsForUserAlertDefinitions(string userId, List<int> installationIds)
            => _context.AlertDefinitions
            .Where(a => a.UserId == userId && installationIds.Contains(a.InstallationId))
            .Select(i => i.InstallationId)
            .ToListAsync();
    }
}
