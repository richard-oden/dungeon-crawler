using System.Collections.Generic;

namespace DungeonCrawler
{
    public class Item : IMappable, INamed, IDescribable
    {
        public string Name {get; protected set;}
        public double Value {get; protected set;}
        public double Weight {get; protected set;}
        public Dictionary<string, int> AbilityMods {get; protected set;}
        public MapPoint Location {get; protected set;}
        public char Symbol {get; protected set;} = Symbols.Item;
        public string Description {get; protected set;}

        public Item(string name, double value, double weight, Dictionary<string, int> abilityMods = null, MapPoint location = null)
        {
            Name = name;
            Value = value;
            Weight = weight;
            abilityMods ??= new Dictionary<string, int>();
            AbilityMods = abilityMods;
            Location = location;
        }

        public void SetLocation(MapPoint location)
        {   
            Location = location;
        }

        public void SetDescription(string description)
        {
            Description = description;
        }
    }
}