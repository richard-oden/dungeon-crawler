using System.Collections.Generic;

namespace DungeonCrawler
{
    public class Consumable : Item
    {
        public int Duration {get; private set;}

        public List<StatusEffect> StatusEffects {get; private set;}

        public Consumable(string name, double value, double weight, List<StatusEffect> statusEffects) : 
            base(name, value, weight, abilityMods: null)
        {
            StatusEffects = statusEffects;
        }
    }
}