using System.Collections.Generic;

namespace DungeonCrawler
{
    class Rogue : Caste
    {
        public TargetedAction Action {get; private set;}
        public Rogue()
        {
            Name = "Rogue";
            AbilityProficiency = "DEX";
            ArmorProficiency = "leather";
            CanUseShield = false;
            WeaponProficiency = new List<string> {"dagger", "bow", "sword"};
        }

        private bool assassinate()
        {
            return true;
        }
    }
}