using System.Collections.Generic;

namespace DungeonCrawler
{
    class Cleric : Caste
    {
        public Cleric()
        {
            Name = "Cleric";
            AbilityProficiency = "WIS";
            ArmorProficiency = "plate";
            CanUseShield = true;
            WeaponProficiency = new List<string> {"flail", "mace", "warhammer"};
        }
    }
}