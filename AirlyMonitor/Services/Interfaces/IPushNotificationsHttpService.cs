using AirlyInfrastructure.Models.Dtos;

namespace AirlyMonitor.Services.Interfaces
{
    public interface IPushNotificationsHttpService
    {
        Task<string> AddDeviceToken(AddDeviceTokenDto deviceToken, string token);
        Task<Dictionary<string, string>> GetAllDeviceTokens(string token);
    }
}
