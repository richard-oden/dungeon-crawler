using System.Collections.Generic;

namespace DungeonCrawler
{
    public class Item
    {
        public string Name {get; protected set;}
        public double Value {get; protected set;}
        public double Weight {get; protected set;}
        public Dictionary<string, int> AbilityMods {get; protected set;}
    }
}