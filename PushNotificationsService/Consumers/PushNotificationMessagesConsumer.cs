using AirlyInfrastructure.Models.Messages;
using MassTransit;
using PushNotificationsService.Services.Interfaces;

namespace PushNotificationsService.Consumers
{
    public class PushNotificationMessagesConsumer : IConsumer<PushNotificationMessage>
    {
        private readonly IFirebaseCloudMessageService _firebaseCloudMessageService;
        private readonly IEmailNotificationService _emailNotificationService;
        private readonly ILogger<PushNotificationMessagesConsumer> _logger;

        public PushNotificationMessagesConsumer(
            IFirebaseCloudMessageService firebaseCloudMessageService,
            IEmailNotificationService emailNotificationService,
            ILogger<PushNotificationMessagesConsumer> logger
            )
        {
            _firebaseCloudMessageService = firebaseCloudMessageService;
            _emailNotificationService = emailNotificationService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<PushNotificationMessage> context)
        {
            _logger.LogInformation($"consumed message for user: {context.Message.UserId}");
            try
            {
                await _firebaseCloudMessageService.SendNotificationAsync(context.Message.UserId, context.Message.Text, context.Message.DetailedMessage);
            } catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            try
            {
                await _emailNotificationService.SendEmailAsync(context.Message);
            } catch (Exception ex) 
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
