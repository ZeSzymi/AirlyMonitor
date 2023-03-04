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
        }

        public async Task SendNotificationAsync<T>(string userId, string title, T body)
        {
            var credentials = GoogleCredential.FromFile("credentials.json");
            FirebaseApp.Create(new AppOptions()
            {
                Credential = credentials,
            });

            var token = _deviceTokens.GetValueOrDefault(userId);

            if (token == null)
            {
                return;
            }

            // Set up the message to be sent
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
                // Send the message
                var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);

                // Print the response
                _logger.LogInformation($"Message sent successfully: {response}");
            }
            catch (FirebaseMessagingException ex)
            {
                // Handle any errors that occur during sending
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
                    // Send a message to the device
                    var response = await FirebaseMessaging.DefaultInstance.SendAsync(new Message()
                    {
                        Token = deviceToken,
                    });

                    _logger.LogInformation($"Device token {deviceToken} is valid.");
                }
                catch (FirebaseMessagingException ex)
                {
                    // If the message sending fails, the device token is no longer valid
                    _logger.LogInformation($"Device token {deviceToken} is invalid. Error message: {ex.Message}");
                    RemoveDeviceToken(deviceTokenEntry.Key);

                    // Remove the invalid device token from your app server's database here
                }
            }
        }

        public void AddDeviceToken(string userId, string deviceToken) => _deviceTokens[userId] = deviceToken;

        public void RemoveDeviceToken(string userId) => _deviceTokens.Remove(userId, out _);
    }
}
