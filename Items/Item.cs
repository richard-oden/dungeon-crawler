using System.Collections.Generic;

namespace DungeonCrawler
{
    public class Item
    {
        public string Name {get; protected set;}
        public double Value {get; protected set;}
        public double Weight {get; protected set;}
        public Dictionary<string, int> AbilityMods {get; protected set;}

        public Item(string name, double value, double weight, Dictionary<string, int> abilityMods = null)
        {
            Name = name;
            Value = value;
            Weight = weight;
            abilityMods ??= new Dictionary<string, int>();
            AbilityMods = abilityMods;
        }
    }
}