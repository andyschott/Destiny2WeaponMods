using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Destiny2;
using Destiny2.Definitions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Destiny2WeaponMods.Services
{
    public class WeaponMods : IWeaponMods
    {
        private readonly IManifest _manifest;
        private readonly IDestiny2 _destiny2;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger _logger;

        private const uint WeaponModsDamageCategoryHash = 1052191496;
        private const uint DummiesCategoryHash = 3109687656;

        public WeaponMods(IManifest manifest, IDestiny2 destiny2,
            IHttpContextAccessor contextAccessor, ILogger<WeaponMods> logger)
        {
            _manifest = manifest;
            _destiny2 = destiny2;
            _contextAccessor = contextAccessor;
            _logger = logger;
        }
        
        public async Task<IEnumerable<DestinyInventoryItemDefinition>> GetModsFromManifest()
        {
            var manifestMods = await _manifest.LoadInventoryItemsWithCategory(WeaponModsDamageCategoryHash);
            var filteredMods = manifestMods.Where(mod =>
                !mod.ItemCategoryHashes.Contains(DummiesCategoryHash) && // some mods for weapon damage have this category (?)
                !mod.DisplayProperties.Description.Contains("deprecated")); // year 1 mods

            return filteredMods;
        }

        public async Task<IEnumerable<DestinyInventoryItemDefinition>> GetModsFromInventory(BungieMembershipType type,
            long accountId)
        {
            var accessToken = await _contextAccessor.HttpContext.GetTokenAsync("access_token");

            var mods = new List<DestinyInventoryItemDefinition>();

            var inventory = await _destiny2.GetProfile(accessToken, type, accountId,
                DestinyComponentType.ProfileInventories);
            foreach(var item in inventory.ProfileInventory.Data.Items)
            {
                var itemDef = await _manifest.LoadInventoryItem(item.ItemHash);
                if(itemDef.ItemCategoryHashes.Contains(WeaponModsDamageCategoryHash))
                {
                    mods.Add(itemDef);
                }
            }

            return mods;
        }
    }
}