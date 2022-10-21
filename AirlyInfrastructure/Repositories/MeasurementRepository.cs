using AirlyInfrastructure.Contexts;
using AirlyInfrastructure.Database;
using AirlyInfrastructure.Repositories.Interfaces;
using AirlyMonitor.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace AirlyInfrastructure.Repositories
{
    public class MeasurementRepository : IMeasurementRepository
    {
        private readonly AirlyDbContext _context;

        public MeasurementRepository(AirlyDbContext context)
        {
            _context = context;
        }

        public async Task<Installation> AddInstallationAsync(Installation installation)
        {
            await _context.Installations.AddAsync(installation);
            await _context.SaveChangesAsync();
            return installation;
        }

        public async Task<List<Measurement>> AddMeasurementsAsync(List<Measurement> measurements)
        {
            _context.Measurements.AddRange(measurements);
            await _context.SaveChangesAsync();
            return measurements;
        }

        public Task<List<int>> GetInstallationIdsAsync() => _context.Installations.Select(i => i.Id).ToListAsync();

    }
}
