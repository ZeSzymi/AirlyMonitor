using AirlyInfrastructure.Models.Dtos;
using AirlyInfrastructure.Models.Messages;
using AirlyMonitor.Models.Dtos;
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
        public async Task<ActionResult> RegisterDeviceToken([FromBody]AddDeviceTokenDto deviceToken)
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            await _pushNotificationsHttpService.AddDeviceToken(deviceToken, token);

            return Ok("token has been added");
        }

        [HttpPost("test-notifications")]
        public async Task<ActionResult> TestNotifications([FromBody]TestNotificationDto testNotificationDto)
        {
            var message = new PushNotificationMessage
            {
                DetailedMessage = "test",
                Text = "test",
                Email = testNotificationDto.Email,
                UserId = User.Identity.Name
            };

            await _publishEndpoint.Publish(message);

            return Ok();
        }

        [HttpGet("device-tokens")]
        public async Task<ActionResult> TestNotifications()
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            var tokens = await _pushNotificationsHttpService.GetAllDeviceTokens(token);

            return Ok(tokens);
        }
    }
}
