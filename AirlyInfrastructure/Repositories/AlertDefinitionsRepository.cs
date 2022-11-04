using AirlyInfrastructure.Contexts;
using AirlyInfrastructure.Database;
using AirlyInfrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AirlyInfrastructure.Repositories
{
    public class AlertDefinitionsRepository : IAlertDefinitionsRepository
    {
        private readonly AirlyDbContext _context;

        public AlertDefinitionsRepository(AirlyDbContext context)
        {
            _context = context;
        }

        public async Task<AlertDefinition> AddAlertDefinitionAsync(AlertDefinition alertDefinition)
        {
            await _context.AlertDefinitions.AddAsync(alertDefinition);
            await _context.SaveChangesAsync();
            return alertDefinition;
        }

        public Task<List<AlertDefinition>> GetAlertDefinitionsAsync()
            => _context.AlertDefinitions.ToListAsync();

        public Task<List<AlertDefinition>> GetAlertDefinitionsAsync(int installationId)
            => _context.AlertDefinitions.Where(a => a.InstallationId == installationId).ToListAsync();
    }
}
