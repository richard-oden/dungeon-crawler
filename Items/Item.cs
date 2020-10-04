using System.Collections.Generic;

namespace DungeonCrawler
{
    public class Item : IMappable
    {
        public string Name {get; protected set;}
        public double Value {get; protected set;}
        public double Weight {get; protected set;}
        public Dictionary<string, int> AbilityMods {get; protected set;}
        public Point Location {get; protected set;}
        public char Symbol {get; protected set;} = Symbols.Item;

        public Item(string name, double value, double weight, Dictionary<string, int> abilityMods = null, Point location = null)
        {
            Name = name;
            Value = value;
            Weight = weight;
            abilityMods ??= new Dictionary<string, int>();
            AbilityMods = abilityMods;
            Location = location;
        }

        public void SetLocation(Point location)
        {   
            Location = location;
        }
    }
}