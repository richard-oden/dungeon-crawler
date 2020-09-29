using System.Collections.Generic;

namespace DungeonCrawler
{
    public class Axe : Weapon
    {
        protected Axe(string name, double value, double weight, bool twoHanded, Die damageDie = null, string damageType = null, Dictionary<string, int> abilityMods = null) : base(name, value, weight, twoHanded, abilityMods)
        {
            TwoHanded = twoHanded;
            Type = "axe";

            damageDie ??= Dice.D10;
            DamageDie = damageDie;

            damageType ??= "slashing";
            DamageType = damageType;

            Range = 5;
        }
    }
}