using System.Collections.Generic;

namespace DungeonCrawler
{
    class Rogue : Caste
    {
        public Rogue()
        {
            Name = "Rogue";
            AbilityProficiency = "DEX";
            ArmorProficiency = "leather";
            CanUseShield = false;
            WeaponProficiency = new List<string> {"dagger", "bow", "sword"};
        }
    }
}