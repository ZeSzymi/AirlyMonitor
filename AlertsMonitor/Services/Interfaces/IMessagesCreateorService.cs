using AirlyInfrastructure.Database;

namespace AlertsMonitor.Services.Interfaces
{
    public interface IMessagesCreateorService
    {
        Task SendMessages(List<Alert> alerts, List<AlertDefinition> alertDefinitions);
    }
}
