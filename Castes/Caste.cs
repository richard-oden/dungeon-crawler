using System;
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
        public SentientCreature SentientCreature {get; set;}
        public virtual IEntityAction Action {get; protected set;}
        
        public static Caste ParseCaste(string caste)
        {
            if (caste == "Rogue") return new Rogue();
            else if (caste == "Cleric") return new Cleric();
            else if (caste == "Fighter") return new Fighter();
            else if (caste == "Wizard") return new Wizard();
            else throw new Exception($"'{caste}' is not a valid caste.");
        }
    }
}