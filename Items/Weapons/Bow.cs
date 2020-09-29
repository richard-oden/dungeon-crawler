using System.Collections.Generic;

namespace DungeonCrawler
{
    public class Bow : Weapon
    {
        protected Bow(string name, double value, double weight, bool twoHanded, Die damageDie = null, string damageType = null, Stat range = null, Dictionary<string, int> abilityMods = null) : base(name, value, weight, twoHanded, abilityMods)
        {
            TwoHanded = twoHanded;
            Type = "bow";

            damageDie ??= Dice.D8;
            DamageDie = damageDie;

            damageType ??= "slashing";
            DamageType = DamageType;
        }
    }
}