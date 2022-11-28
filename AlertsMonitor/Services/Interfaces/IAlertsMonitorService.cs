namespace AlertsMonitor.Services.Interfaces
{
    public interface IAlertsMonitorService
    {
        Task EvaluateAlerts(DateTime now);
    }
}
