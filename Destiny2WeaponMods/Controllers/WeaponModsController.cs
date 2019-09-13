using System.Linq;
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
            var inventoryModsTask = _weaponMods.GetModsFromInventory(type, accountId);
            var manifestModsTask = _weaponMods.GetModsFromManifest();

            var modsTask = await Task.WhenAll(inventoryModsTask, manifestModsTask);

            var model = new WeaponModsIndexViewModel
            {
                WeaponMods = modsTask[1].OrderBy(mod => mod.DisplayProperties.Name),
            };
            return View(model);
        }
    }
}