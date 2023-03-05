using AirlyInfrastructure.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PushNotificationsService.Services.Interfaces;

namespace PushNotificationsService.Controllers
{
    [Route("/api/[controller]")]
    [Authorize]
    public class TokenController : Controller
    {
        private readonly IFirebaseCloudMessageService _firebaseCloudMessageService;

        public TokenController(IFirebaseCloudMessageService firebaseCloudMessageService)
        {
            _firebaseCloudMessageService = firebaseCloudMessageService;
        }

        [HttpPost]
        public IActionResult AddDeviceToken([FromBody] AddDeviceTokenDto tokenDto)
        {
            _firebaseCloudMessageService.AddDeviceToken(User.Identity.Name, tokenDto.Token);
            return Ok("Token added");
        }

        [HttpGet("all")]
        public IActionResult Get()
        {
            var deviceTokens = _firebaseCloudMessageService.GetDeviceTokens();
            return Ok(deviceTokens);
        }
    }
}
