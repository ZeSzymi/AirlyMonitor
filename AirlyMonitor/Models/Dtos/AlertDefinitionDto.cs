using AirlyInfrastructure.Database;

namespace AirlyMonitor.Models.Dtos
{
    public class AlertDefinitionDto
    {
        public Guid? Id { get; set; }
        public int InstallationId { get; set; }
        public int? CheckEvery { get; set; }
        public List<AlertRule> Rules { get; set; }
        public double? AQIThreshold { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

        public AlertDefinition ToAlertDefinition(string userId) => new()
        {
            UserId = userId,
            Id = Id ?? Guid.Empty,
            InstallationId = InstallationId,
            CheckEvery = CheckEvery ?? 30,
            AlertRules = Rules ?? new List<AlertRule>(),
            AQIThreshold = AQIThreshold,
            From = From,
            To = To
        };
    }
}
