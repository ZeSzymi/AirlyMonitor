﻿using AirlyInfrastructure.Database;

namespace AirlyMonitor.Models.Dtos
{
    public class AlertDefinitionDto
    {
        public Guid? Id { get; set; }
        public int InstallationId { get; set; }
        public int CheckEvery { get; set; }
        public List<AlertRule> Rules { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

        public AlertDefinition ToAlertDefinition() => new()
        {
            Id = Id ?? Guid.Empty,
            InstallationId = InstallationId,
            CheckEvery = CheckEvery,
            AlertRules = Rules,
            From = From,
            To = To
        };
    }
}