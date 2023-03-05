using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;
using PushNotificationsService.Services.Interfaces;
using System.Collections.Concurrent;

namespace PushNotificationsService.Services
{
    public class FireBaseCloudMessagesService : IFirebaseCloudMessageService
    {
        private readonly ConcurrentDictionary<string, string> _deviceTokens = new();
        private readonly ILogger<FireBaseCloudMessagesService> _logger;

        public FireBaseCloudMessagesService(ILogger<FireBaseCloudMessagesService> logger)
        {
            _logger = logger;
            var credentials = GoogleCredential.FromFile("credentials.json");
            FirebaseApp.Create(new AppOptions()
            {
                Credential = credentials,
            });
        }

        public async Task SendNotificationAsync<T>(string userId, string title, T body)
        {
            var token = _deviceTokens.GetValueOrDefault(userId);

            if (token == null)
            {
                return;
            }

            var message = new Message()
            {
                Notification = new Notification()
                {
                    Title = title,
                    Body = JsonConvert.SerializeObject(body)
                },
                Token = token
            };

            try
            {
                var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                _logger.LogInformation($"Message sent successfully: {response}");
            }
            catch (FirebaseMessagingException ex)
            {
                _logger.LogError($"Error sending message: {ex.Message}", ex);
            }
        }

        public async Task CheckDeviceTokenValidityAsync()
        {
            foreach (var deviceTokenEntry in _deviceTokens)
            {
                var deviceToken = deviceTokenEntry.Value;

                try
                {
                    var response = await FirebaseMessaging.DefaultInstance.SendAsync(new Message()
                    {
                        Token = deviceToken,
                    });

                    _logger.LogInformation($"Device token {deviceToken} is valid.");
                }
                catch (FirebaseMessagingException ex)
                {
                    _logger.LogInformation($"Device token {deviceToken} is invalid. Error message: {ex.Message}");
                    RemoveDeviceToken(deviceTokenEntry.Key);
                }
            }
        }

        public void AddDeviceToken(string userId, string deviceToken) => _deviceTokens[userId] = deviceToken;

        public void RemoveDeviceToken(string userId) => _deviceTokens.Remove(userId, out _);

        public Dictionary<string, string> GetDeviceTokens() => _deviceTokens.ToDictionary(kv => kv.Key, kv => kv.Value);
    }
}
