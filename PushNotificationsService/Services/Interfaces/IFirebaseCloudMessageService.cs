namespace PushNotificationsService.Services.Interfaces
{
    public interface IFirebaseCloudMessageService
    {
        Task SendNotificationAsync<T>(string userId, string title, T body);
        void AddDeviceToken(string userId, string deviceToken);
        void RemoveDeviceToken(string userId);
        Task CheckDeviceTokenValidityAsync();
    }
}
