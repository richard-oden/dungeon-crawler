using System.Collections.Generic;

namespace DungeonCrawler
{
    public class TorsoArmor : Armor
    {
        public TorsoArmor(string name, double value, double weight, int ac, string material, Dictionary<string, int> abilityMods = null) : base(name, value, weight, ac, material, abilityMods)
        {
            Slot = "Torso";
        }
    }
}