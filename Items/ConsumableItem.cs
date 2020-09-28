using System.Collections.Generic;

namespace DungeonCrawler
{
    public class Consumable : Item
    {
        public int Duration {get; private set;}

        protected Consumable(string name, double value, double weight, int duration, Dictionary<string, int> abilityMods = null) : base(name, value, weight, abilityMods)
        {
            Duration = duration;
        }
    }
}