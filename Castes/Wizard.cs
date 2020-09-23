using System.Collections.Generic;

namespace DungeonCrawler
{
    class Wizard : Caste
    {
        public Wizard()
        {
            Name = "Wizard";
            AbilityProficiency = "INT";
            ArmorProficiency = "cloth";
            CanUseShield = false;
            WeaponProficiency = new List<string> {"staff", "wand", "orb"};
        }
    }
}