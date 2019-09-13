using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Destiny2WeaponMods.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class WeaponModsController : Controller
    {
        [HttpGet(Name = "WeaponModsIndex")]
        public IActionResult Index()
        {
            return View();
        }
    }
}