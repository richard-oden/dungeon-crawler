using System.Collections.Generic;

namespace DungeonCrawler
{
    public class Consumable : Item
    {

        public List<StatusEffect> StatusEffects {get; private set;}

        public Consumable(string name, double value, double weight, List<StatusEffect> statusEffects) : 
            base(name, value, weight, abilityMods: null)
        {
            StatusEffects = statusEffects;
        }

        public Consumable(Item baseItem, List<StatusEffect> statusEffects) :
            this(baseItem.Name, baseItem.Value, baseItem.Weight, statusEffects)
        {}
    }
}