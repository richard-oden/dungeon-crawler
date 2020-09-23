using System.Collections.Generic;

namespace DungeonCrawler
{
    class Barbarian : Caste
    {
        public Barbarian()
        {
            Name = "Barbarian";
            AbilityProficiency = "CON";
            ArmorProficiency = "leather";
            CanUseShield = false;
            WeaponProficiency = new List<string> {"maul", "axe", "gloves"};
        }
    }
}