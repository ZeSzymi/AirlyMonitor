using System.Security.Claims;
using CryptoHelper;
using IdentityServer.Models.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;

namespace IdentityServer.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IOpenIddictApplicationManager _manager;

        public AccountController(UserManager<IdentityUser> userManager, IOpenIddictApplicationManager manager)
        {
            _userManager = userManager;
            _manager = manager;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Username);

            if (user == null)
            {
                return BadRequest("User does not exist");
            }

            var result = await _userManager.CheckPasswordAsync(user, model.Password);

            if (result)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Id),
                    new Claim(ClaimTypes.NameIdentifier, model.Username)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return Ok(new { ReturnUrl = $"http://{HttpContext.Request.Host}{model.ReturnUrl}" } );
            }
           
            return BadRequest("Wrong password");
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            var result = await _userManager.FindByEmailAsync(model.Username);
            if (result == null)
            {
                var registrationResult = await _userManager.CreateAsync(new IdentityUser
                {
                    UserName = model.Username,
                    Email = model.Username,
                }, model.Password);

                if (registrationResult.Succeeded)
                { 
                    return Ok();
                }
            }

            return BadRequest();
        }

        [HttpGet("add/client")]
        public async Task<IActionResult> client()
        {
            if (await _manager.FindByClientIdAsync("postman") is null)
            {
                await _manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "postman",
                    ClientSecret = "postman-secret",
                    DisplayName = "Postman",
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,

                        OpenIddictConstants.Permissions.Prefixes.Scope + "api"
                    }
                });
            }

            return BadRequest();
        }

        [HttpGet("~/pass/{pass}")]
        public async Task<IActionResult> client(string pass)
        {
            return Ok(Crypto.HashPassword(pass));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return Ok();
        }
    }
}