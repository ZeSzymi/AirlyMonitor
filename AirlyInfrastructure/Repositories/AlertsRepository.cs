using AirlyInfrastructure.Contexts;
using AirlyInfrastructure.Database;
using AirlyInfrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AirlyInfrastructure.Repositories
{
    public class AlertsRepository : IAlertsRepository
    {
        private readonly AirlyDbContext _airlyDbContext;

        public AlertsRepository(AirlyDbContext airlyDbContext)
        {
            _airlyDbContext = airlyDbContext;
        }

        public async Task<List<Alert>> AddAlertsAsync(List<Alert> alerts)
        {
            _airlyDbContext.Alerts.AddRange(alerts);
            await _airlyDbContext.SaveChangesAsync();
            return alerts;
        }

        public Task<List<Alert>> GetAlertsAsync()
            => _airlyDbContext.Alerts.ToListAsync();

        public Task<List<Alert>> GetAlertsForAlertDefinitionId(Guid alarmDefinitionId)
            => _airlyDbContext.Alerts.Where(a => a.AlertDefinitionId == alarmDefinitionId).ToListAsync();

        public Task<List<Alert>> GetLatestAlertsAsync(List<Guid> alarmDefinitionIds)
            => _airlyDbContext.Alerts           
            .Where(a => alarmDefinitionIds.Contains(a.AlertDefinitionId))
            .GroupBy(a => a.AlertDefinitionId)
            .Select(ms => ms.OrderByDescending(m => m.DateTime).First())
            .ToListAsync();
    }
}
