using System.Collections.Generic;

namespace DungeonCrawler
{
    public class Bow : Weapon
    {
        public Bow(string name, double value, double weight, Die damageDie = null, string damageType = null, Dictionary<string, int> abilityMods = null, int attackBonus = 0, int damageBonus = 0) : 
                base(name, value, weight, twoHanded: true, abilityMods, attackBonus, damageBonus)
        {
            Type = "bow";

            damageDie ??= Dice.D8;
            DamageDie = damageDie;

            damageType ??= "piercing";
            DamageType = damageType;

            Range = 80;
        }
    }
}