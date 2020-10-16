using System.Collections.Generic;

namespace DungeonCrawler
{
    public class Dagger : Weapon
    {
        public Dagger(string name, double value, double weight, Die damageDie = null, string damageType = null, Dictionary<string, int> abilityMods = null, int attackBonus = 0, int damageBonus = 0) : 
            base(name, value, weight, twoHanded: false, abilityMods, attackBonus, damageBonus)
        {
            Type = "dagger";

            damageDie ??= Dice.D6;
            DamageDie = damageDie;

            damageType ??= "slashing";
            DamageType = damageType;

            Range = 5;
        }
    }
}