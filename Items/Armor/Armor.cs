using System.Collections.Generic;

namespace DungeonCrawler
{
    public class Armor : Item
    {
        public int ArmorClassBonus {get; protected set;}
        public string Material {get; protected set;}
        public string Slot {get; protected set;}
        public Armor( string slot, string name, double value, double weight, int ac, string material, Dictionary<string, int> abilityMods = null) : base(name, value, weight, abilityMods)
        {
            ArmorClassBonus = ac;
            Material = material;
            AbilityMods = abilityMods;
            Slot = slot;
        }
    }
}