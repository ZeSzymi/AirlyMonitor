using AirlyInfrastructure.Database;
using AirlyInfrastructure.Models.Messages;
using AirlyInfrastructure.Repositories.Interfaces;
using AlertsMonitor.Services.Interfaces;
using MassTransit;

namespace AlertsMonitor.Services
{
    public class MessagesCreatorService : IMessagesCreateorService
    {
        private readonly IInstallationsRepository _installationsRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public MessagesCreatorService(
            IInstallationsRepository installationsRepository,
            IUsersRepository usersRepository,
            IPublishEndpoint publishEndpoint)
        {
            _installationsRepository = installationsRepository;
            _usersRepository = usersRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task SendMessages(List<Alert> alerts, List<AlertDefinition> alertDefinitions)
        {
            var alertDefinitionIds = alerts.Select(alert => alert.AlertDefinitionId).Distinct().ToList();
            var filteredAlertDefinitions = alertDefinitions.Where(ad => alertDefinitionIds.Contains(ad.Id)).ToList();
            var installationIds = filteredAlertDefinitions.Select(fad => fad.InstallationId).Distinct().ToList();
            var userIds = filteredAlertDefinitions.Select(fad => fad.UserId).Distinct().ToList();

            var installations = await _installationsRepository.GetInstallationsAsync(installationIds);
            var users = await _usersRepository.GetUsersAsync(userIds);

            foreach (var alert in alerts.Where(alert => alert.RaiseAlert == true && alert.PreviousRaisedAlert == false))
            {
                var alertDefinition = filteredAlertDefinitions.First(alertDefinition => alertDefinition.Id == alert.AlertDefinitionId);
                var installation = installations.First(installation => installation.Id == alertDefinition.InstallationId);
                var user = users.First(user => user.Id == alertDefinition.UserId);
                var alertReports = alert.AlertReports.Where(alertReport => alertReport.RaiseAlert == true);
                var firstAlertReport = alertReports.FirstOrDefault() ?? alert.AQIAlertReports;

                var messageText = $"Installation in {installation.Address.DisplayAddress1} {installation.Address.DisplayAddress2} has raised alert for {firstAlertReport.MeasurementName.ToLower()}";
                var detailedMessage = string.Empty;

                foreach (var alertReport in alertReports)
                {
                    detailedMessage += $"${alertReport.GetReportMessage()}\n";
                }

                var message = new PushNotificationMessage
                {
                    DetailedMessage = detailedMessage,
                    Email = user.Email,
                    UserId = user.Id,
                    Text = messageText
                };

                await _publishEndpoint.Publish(message);
            }
        }
    }
}
