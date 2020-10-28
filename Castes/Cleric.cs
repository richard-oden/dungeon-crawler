using System.Collections.Generic;

namespace DungeonCrawler
{
    class Cleric : Caste
    {
        public NonTargetedAction Action {get; private set;}
        public Cleric()
        {
            Name = "Cleric";
            AbilityProficiency = "WIS";
            ArmorProficiency = "plate";
            CanUseShield = true;
            WeaponProficiency = new List<string> {"flail", "mace", "warhammer"};
        }

        private bool prayer()
        {
            return true;
        }
    }
}