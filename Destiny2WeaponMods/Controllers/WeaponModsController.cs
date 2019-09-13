using System.Linq;
using System.Threading.Tasks;
using Destiny2;
using Destiny2.Definitions;
using Destiny2WeaponMods.Helpers;
using Destiny2WeaponMods.Models;
using Destiny2WeaponMods.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Destiny2WeaponMods.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class WeaponModsController : Controller
    {
        private readonly IWeaponMods _weaponMods;
        private readonly BungieSettings _bungie;

        public WeaponModsController(IWeaponMods weaponMods, IOptions<BungieSettings> bungie)
        {
            _weaponMods = weaponMods;
            _bungie = bungie.Value;
        }

        [HttpGet("{type}/{accountId}", Name = "WeaponModsIndex")]
        public async Task<IActionResult> Index(BungieMembershipType type, long accountId)
        {
            var inventoryModsTask = _weaponMods.GetModsFromInventory(type, accountId);
            var manifestModsTask = _weaponMods.GetModsFromManifest();

            var modsTask = await Task.WhenAll(inventoryModsTask, manifestModsTask);

            var inventoryMods = modsTask[0].ToDictionary(mod => mod.Hash);

            var weaponMods = modsTask[1].Select(mod => new WeaponModViewModel
            {
                IconUrl = GetIconUrl(mod),
                Name = mod.DisplayProperties.Name,
                IsUnlocked = inventoryMods.ContainsKey(mod.Hash),
            }).OrderBy(mod => mod.Name);
            var model = new WeaponModsIndexViewModel
            {
                WeaponMods = weaponMods,
            };
            return View(model);
        }

        private string GetIconUrl(DestinyInventoryItemDefinition itemDefinition)
        {
            if(!itemDefinition.DisplayProperties.HasIcon)
            {
                return string.Empty;
            }

            return _bungie.BaseUrl + itemDefinition.DisplayProperties.Icon;
        }
    }
}