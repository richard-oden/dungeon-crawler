using System.Collections.Generic;

namespace DungeonCrawler
{
    public class Boots : Armor
    {
        public Boots(string name, double value, double weight, int ac, string material, Dictionary<string, int> abilityMods = null) : base(name, value, weight, ac, material, abilityMods)
        {
            Slot = "Feet";
        }
    }
}