namespace AirlyInfrastructure.Models.Messages
{
    public class PushNotificationMessage
    {
        public string UserId { get; set; }
        public string Text { get; set; }
        public string DetailedMessage { get; set; }
        public string Email { get; set; }
    }
}
