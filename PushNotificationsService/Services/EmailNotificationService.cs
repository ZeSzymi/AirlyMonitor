using System.Net.Mail;
using System.Net;
using PushNotificationsService.Options;
using Microsoft.Extensions.Options;
using AirlyInfrastructure.Models.Messages;
using PushNotificationsService.Services.Interfaces;

namespace PushNotificationsService.Services
{
    public class EmailNotificationService : IEmailNotificationService
    {
        private readonly ILogger<EmailNotificationService> _logger;
        private readonly EmailOptions _emailOptions;

        public EmailNotificationService(ILogger<EmailNotificationService> logger, IOptions<EmailOptions> options)
        {
            _logger = logger;
            _emailOptions = options.Value;
        }

        public async Task<bool> SendEmailAsync(PushNotificationMessage pushNotificationMessage)
        {
            var smtpClient = new SmtpClient(_emailOptions.SmtpClient, 587);
            smtpClient.Credentials = new NetworkCredential(_emailOptions.Sender, _emailOptions.Password);
            smtpClient.EnableSsl = true;

            var mailMessage = new MailMessage(_emailOptions.Sender, pushNotificationMessage.Email, pushNotificationMessage.Text, pushNotificationMessage.DetailedMessage);
            mailMessage.IsBodyHtml = true;

            try
            {
                _logger.LogInformation($"Sending email to {pushNotificationMessage.Email}");
                await smtpClient.SendMailAsync(mailMessage);

                return true;
            }
            catch (SmtpException ex)
            {
                _logger.LogError($"Failed to send email: {ex.Message}", ex);
                return false;
            }
        }
    }
}
