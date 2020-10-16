using System.Collections.Generic;

namespace DungeonCrawler
{
    public class Tome : Weapon
    {
        public Tome(string name, double value, double weight, Die damageDie = null, string damageType = null, Dictionary<string, int> abilityMods = null, int attackBonus = 0, int damageBonus = 0) : 
                base(name, value, weight, twoHanded: true, abilityMods, attackBonus, damageBonus)
        {
            Type = "tome";

            damageDie ??= Dice.D12;
            DamageDie = damageDie;

            damageType ??= "magic";
            DamageType = damageType;

            Range = 60;
        }
    }
}