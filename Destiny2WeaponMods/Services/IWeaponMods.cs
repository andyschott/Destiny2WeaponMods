using System.Collections.Generic;
using System.Threading.Tasks;
using Destiny2;
using Destiny2.Definitions;

namespace Destiny2WeaponMods.Services
{
    public interface IWeaponMods
    {
         Task<IEnumerable<DestinyInventoryItemDefinition>> GetModsFromManifest();
         Task<IEnumerable<DestinyInventoryItemDefinition>> GetModsFromInventory(BungieMembershipType type,
            long accountId);
    }
}