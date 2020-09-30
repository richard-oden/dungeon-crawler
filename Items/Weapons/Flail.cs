using System.Collections.Generic;

namespace DungeonCrawler
{
    public class Flail : Weapon
    {
        protected Flail(string name, double value, double weight, bool twoHanded, Die damageDie = null, string damageType = null, Dictionary<string, int> abilityMods = null, int attackBonus = 0, int damageBonus = 0) : base(name, value, weight, twoHanded, abilityMods, attackBonus, damageBonus)
        {
            Type = "flail";

            damageDie ??= Dice.D8;
            DamageDie = damageDie;

            damageType ??= "bludgeoning";
            DamageType = damageType;

            Range = 10;
        }
    }
}