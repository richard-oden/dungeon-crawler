using System.Collections.Generic;

namespace DungeonCrawler
{
    class Wizard : Caste
    {
        public TargetedAction Action {get; private set;}
        public Wizard()
        {
            Name = "Wizard";
            AbilityProficiency = "INT";
            ArmorProficiency = "cloth";
            CanUseShield = false;
            WeaponProficiency = new List<string> {"staff", "wand", "tome"};
        }

        private bool slow()
        {
            return true;
        }
    }
}