using System.Collections.Generic;

namespace DungeonCrawler
{
    public abstract class Caste
    {
        public string Name {get; protected set;}
        public string AbilityProficiency {get; protected set;}
        public string ArmorProficiency {get; protected set;}
        public bool CanUseShield {get; protected set;}
        public List<string> WeaponProficiency {get; protected set;}
        
        public string GetDescription()
        {
            string weapons = "";
            for (int i = 0; i < WeaponProficiency.Count; i++)
            {
                weapons += WeaponProficiency[i] + "s";
                if (i != WeaponProficiency.Count - 1) weapons += ", ";
                if (i == WeaponProficiency.Count - 2) weapons += "and ";
            }
            string useShield = CanUseShield ? "can" : "cannot";
            return $"{Name}s rely on {AbilityProficiency}, and may use {weapons}. They are proficient in {ArmorProficiency} armor, and {useShield} use shields.";
        }
    }
}