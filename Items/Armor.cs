using System.Collections.Generic;

namespace DungeonCrawler
{
    public class Armor : Item
    {
        public int ArmorClassBonus {get; protected set;}
        public string Material {get; protected set;}
        public string Slot {get; protected set;}
        public Armor(string slot, string name, double value, double weight, int ac, string material, 
                    Dictionary<string, int> abilityMods = null, string descriptionText = null) : 
                    base(name, value, weight, abilityMods, descriptionText)
        {
            ArmorClassBonus = ac;
            Material = material;
            AbilityMods = abilityMods;
            Slot = slot;
            Description = DescriptionText + $" It's considered {Material} armor and grants {ArmorClassBonus} AC when worn.";
        }

        public Armor(Item baseItem, string slot, int ac, string material) :
                this(slot, baseItem.Name, baseItem.Value, baseItem.Weight, ac, material,
                    baseItem.AbilityMods, baseItem.DescriptionText)
        {}
    }
}