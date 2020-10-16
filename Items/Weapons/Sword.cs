using System.Collections.Generic;

namespace DungeonCrawler
{
    public class Sword : Weapon
    {
        public Sword(string name, double value, double weight, bool twoHanded, Die damageDie = null, string damageType = null, Dictionary<string, int> abilityMods = null, int attackBonus = 0, int damageBonus = 0) : 
                base(name, value, weight, twoHanded, abilityMods, attackBonus, damageBonus)
        {
            Type = "sword";

            damageDie ??= Dice.D8;
            DamageDie = damageDie;

            damageType ??= "slashing";
            DamageType = damageType;

            Range = 5;
        }
    }
}