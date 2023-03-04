using AirlyInfrastructure.Models.Messages;
using AirlyMonitor.Services.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirlyMonitor.Controllers
{
    [Route("/api/[controller]")]
    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IPushNotificationsHttpService _pushNotificationsHttpService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NotificationsController(
            IPublishEndpoint publishEndpoint, 
            IPushNotificationsHttpService pushNotificationsHttpService,
            IHttpContextAccessor httpContextAccessor)
        {
            _publishEndpoint = publishEndpoint;
            _pushNotificationsHttpService = pushNotificationsHttpService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("register-device")]
        public async Task<ActionResult> RegisterDeviceToken(string deviceToken)
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            await _pushNotificationsHttpService.AddDeviceToken(deviceToken, token);

            return Ok("token has been added");
        }

        [HttpGet("test-notifications")]
        public async Task<ActionResult> TestNotifications()
        {
            var message = new PushNotificationMessage
            {
                DetailedMessage = "test",
                Text = "test",
                Email = "test@test.pl",
                UserId = User.Identity.Name
            };

            await _publishEndpoint.Publish(message);

            return Ok();
        }
    }
}
