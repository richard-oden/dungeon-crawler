using System.Collections.Generic;

namespace DungeonCrawler
{
    public class Mace : Weapon
    {
        public Mace(string name, double value, double weight, bool twoHanded, Die damageDie = null, string damageType = null, Dictionary<string, int> abilityMods = null, int attackBonus = 0, int damageBonus = 0) : 
                base(name, value, weight, twoHanded, abilityMods, attackBonus, damageBonus)
        {
            Type = "mace";

            damageDie ??= Dice.D10;
            DamageDie = damageDie;

            damageType ??= "bludgeoning";
            DamageType = damageType;

            Range = 5;
        }
    }
}