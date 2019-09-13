using Destiny2WeaponMods.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Destiny2WeaponMods.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly ILogger _logger;
        private readonly BungieSettings _bungie;

        public AccountController(ILogger<AccountController> logger, IOptions<BungieSettings> bungie)
        {
            _logger = logger;
            _bungie = bungie.Value;
        }
        
        [HttpGet("login")]
        public IActionResult Login(string returnUrl = "/")
        {
            _logger.LogInformation("Login");
            return Challenge(new AuthenticationProperties() { RedirectUri = returnUrl });
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            _logger.LogInformation("Logut");
            Response.Cookies.Delete(_bungie.LoginCookieName);

            var url = Url.Action("Index", "Home");
            return Redirect(url);
        }
    }
}