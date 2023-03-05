using AirlyInfrastructure.Models.Dtos;
using AirlyMonitor.Services.Interface;
using AirlyMonitor.Services.Interfaces;

namespace AirlyMonitor.Services
{
    public class PushNotificationsHttpService : IPushNotificationsHttpService
    {
        private readonly IHttpService _httpService;
        private readonly string _pushNotificationsUrl = "http://localhost:5013";

        public PushNotificationsHttpService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public Task<string> AddDeviceToken(AddDeviceTokenDto deviceToken, string token)
            => _httpService.Post<string, AddDeviceTokenDto>($"{_pushNotificationsUrl}/api/token", deviceToken, token);
        public Task<Dictionary<string, string>> GetAllDeviceTokens(string token)
            => _httpService.Get<Dictionary<string, string>>($"{_pushNotificationsUrl}/api/token/all", token);
    }
}
