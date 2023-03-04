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
        public IActionResult Get(string token)
        {
            _firebaseCloudMessageService.AddDeviceToken(User.Identity.Name, token);
            return Ok("Token added");
        }
    }
}
