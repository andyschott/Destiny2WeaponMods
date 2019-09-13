using System.Collections.Generic;
using Destiny2.Definitions;

namespace Destiny2WeaponMods.Models
{
    public class WeaponModsIndexViewModel
    {
        public IEnumerable<DestinyInventoryItemDefinition> WeaponMods { get; set; }
    }
}