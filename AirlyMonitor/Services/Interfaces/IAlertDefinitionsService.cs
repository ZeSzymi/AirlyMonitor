﻿using AirlyInfrastructure.Database;

namespace AirlyMonitor.Services.Interfaces
{
    public interface IAlertDefinitionsService
    {
        Task<AlertDefinition> AddAlertDefinitionAsync(AlertDefinition alertDefinition);
        Task<List<AlertDefinition>> GetAlertDefinitionsAsync(int installationId);
        Task<List<AlertDefinition>> GetAlertDefinitionsForUserAsync(string userId);
        Task<List<AlertDefinition>> GetAlertDefinitionsAsync();
        Task RemoveAlertDefinitionAsync(int installationId);
    }
}
