using System.Collections.Generic;

namespace DungeonCrawler
{
    class Bard : Caste
    {
        public Bard()
        {
            Name = "Bard";
            AbilityProficiency = "CHA";
            ArmorProficiency = "cloth";
            CanUseShield = false;
            WeaponProficiency = new List<string> {"lute", "cornett", "tabor"};
        }
    }
}