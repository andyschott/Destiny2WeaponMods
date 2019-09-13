using System.Threading.Tasks;
using Destiny2;
using Destiny2WeaponMods.Models;
using Destiny2WeaponMods.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Destiny2WeaponMods.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class WeaponModsController : Controller
    {
        private readonly IWeaponMods _weaponMods;

        public WeaponModsController(IWeaponMods weaponMods)
        {
            _weaponMods = weaponMods;
        }

        [HttpGet("{type}/{accountId}", Name = "WeaponModsIndex")]
        public async Task<IActionResult> Index(BungieMembershipType type, long accountId)
        {
            var inventoryMods = await _weaponMods.GetModsFromInventory(type, accountId);

            var model = new WeaponModsIndexViewModel
            {
                WeaponMods = inventoryMods,
            };
            return View(model);
        }
    }
}