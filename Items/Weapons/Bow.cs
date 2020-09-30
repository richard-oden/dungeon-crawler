using System.Collections.Generic;

namespace DungeonCrawler
{
    public class Bow : Weapon
    {
        protected Bow(string name, double value, double weight, bool twoHanded, Die damageDie = null, string damageType = null, Dictionary<string, int> abilityMods = null, int attackBonus = 0, int damageBonus = 0) : base(name, value, weight, twoHanded, abilityMods, attackBonus, damageBonus)
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