using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Destiny2;
using Destiny2.Definitions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Destiny2WeaponMods.Services
{
    public class WeaponMods : IWeaponMods
    {
        private readonly IManifest _manifest;
        private readonly IDestiny2 _destiny2;
        private readonly IHttpContextAccessor _contextAccessor;

        private const uint WeaponModsCategoryHash = 610365472;

        public WeaponMods(IManifest manifest, IDestiny2 destiny2,
            IHttpContextAccessor contextAccessor)
        {
            _manifest = manifest;
            _destiny2 = destiny2;
            _contextAccessor = contextAccessor;
        }
        
        public Task<IEnumerable<DestinyInventoryItemDefinition>> GetModsFromManifest()
        {
            throw new System.NotImplementedException();
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
                if(itemDef.ItemCategoryHashes.Contains(WeaponModsCategoryHash))
                {
                    mods.Add(itemDef);
                }
            }

            return mods;
        }
    }
}