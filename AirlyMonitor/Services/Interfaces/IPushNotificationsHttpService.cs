namespace AirlyMonitor.Services.Interfaces
{
    public interface IPushNotificationsHttpService
    {
        Task<string> AddDeviceToken(string deviceToken, string token);
    }
}
