using System.Collections.Generic;

namespace DungeonCrawler
{
    class Fighter : Caste
    {
        public Fighter()
        {
            Name = "Fighter";
            AbilityProficiency = "STR";
            ArmorProficiency = "plate";
            CanUseShield = true;
            WeaponProficiency = new List<string> {"sword", "axe", "warhammer"};
        }
    }
}