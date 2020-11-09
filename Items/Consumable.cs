using System.Collections.Generic;

namespace DungeonCrawler
{
    public class Consumable : Item
    {

        public List<StatusEffect> StatusEffects {get; private set;}

        public Consumable(string name, double value, double weight, List<StatusEffect> statusEffects, string descriptionText) : 
            base(name, value, weight, abilityMods: null, descriptionText)
        {
            StatusEffects = statusEffects;
        }

        public Consumable(Item baseItem, List<StatusEffect> statusEffects) :
            this(baseItem.Name, baseItem.Value, baseItem.Weight, statusEffects, baseItem.DescriptionText)
        {}
    }
}