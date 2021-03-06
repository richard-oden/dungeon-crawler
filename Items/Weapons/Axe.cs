using System.Collections.Generic;

namespace DungeonCrawler
{
    public class Axe : Weapon
    {
        public Axe(string name, double value, double weight, bool twoHanded, Die damageDie = null, string damageType = null, Dictionary<string, int> abilityMods = null, int attackBonus = 0, int damageBonus = 0) : 
                base(name, value, weight, twoHanded: true, abilityMods, attackBonus, damageBonus)
        {
            Type = "axe";

            damageDie ??= Dice.D10;
            DamageDie = damageDie;

            damageType ??= "slashing";
            DamageType = damageType;

            Range = 5;
        }
    }
}