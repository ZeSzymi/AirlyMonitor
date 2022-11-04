using AirlyInfrastructure.Contexts;
using AirlyInfrastructure.Database;
using AirlyInfrastructure.Repositories.Interfaces;
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

        public async Task<List<Measurement>> AddMeasurementsAsync(List<Measurement> measurements)
        {
            _context.Measurements.AddRange(measurements);
            await _context.SaveChangesAsync();
            return measurements;
        }

        public Task<List<Measurement>> GetMeasurementsAsync(List<int> installationsIds)
            => _context.Measurements
            .Where(m => installationsIds.Contains(m.InstallationId))
            .GroupBy(m => m.InstallationId)
            .Select(ms => ms.OrderByDescending(m => m.FromDateTime).First())
            .ToListAsync();

        public Task<List<Measurement>> GetMeasurementsAsync(int installationId) =>
            _context.Measurements
                .Where(m => m.InstallationId == installationId)
                .ToListAsync();
    }
}
