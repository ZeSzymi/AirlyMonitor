using AirlyMonitor.Services.Interface;
using AirlyMonitor.Services.Interfaces;

namespace AirlyMonitor.Services
{
    public class PushNotificationsHttpService : IPushNotificationsHttpService
    {
        private readonly IHttpService _httpService;
        private readonly string _pushNotificationsUrl = "http://localhost:5013/api/token";

        public PushNotificationsHttpService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public Task<string> AddDeviceToken(string deviceToken, string token)
            => _httpService.Post<string, string>(_pushNotificationsUrl, deviceToken, token);

    }
}
