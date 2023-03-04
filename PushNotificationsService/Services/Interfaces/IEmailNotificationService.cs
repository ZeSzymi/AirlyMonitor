using AirlyInfrastructure.Models.Messages;

namespace PushNotificationsService.Services.Interfaces
{
    public interface IEmailNotificationService
    {
        Task<bool> SendEmailAsync(PushNotificationMessage pushNotificationMessage);
    }
}
