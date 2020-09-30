using System.Collections.Generic;

namespace DungeonCrawler
{
    public class Wand : Weapon
    {
        protected Wand(string name, double value, double weight, bool twoHanded, Die damageDie = null, string damageType = null, Dictionary<string, int> abilityMods = null, int attackBonus = 0, int damageBonus = 0) : base(name, value, weight, twoHanded, abilityMods, attackBonus, damageBonus)
        {
            Type = "tome";

            damageDie ??= Dice.D6;
            DamageDie = damageDie;

            damageType ??= "magic";
            DamageType = damageType;

            Range = 80;
        }
    }
}