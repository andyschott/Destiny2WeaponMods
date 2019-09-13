using Destiny2;

namespace Destiny2WeaponMods.Models
{
    public class Account
    {
        public Account(BungieMembershipType type, long id)
        {
            Id = id;
            Type = type;
        }
        
        public long Id { get; }
        public BungieMembershipType Type { get; }
    }
}