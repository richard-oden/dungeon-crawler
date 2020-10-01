using System.Collections.Generic;

namespace DungeonCrawler
{
    public class Warhammer : Weapon
    {
        public Warhammer(string name, double value, double weight, bool twoHanded, Die damageDie = null, string damageType = null, Dictionary<string, int> abilityMods = null, int attackBonus = 0, int damageBonus = 0) : base(name, value, weight, twoHanded, abilityMods, attackBonus, damageBonus)
        {
            Type = "warhammer";

            damageDie ??= Dice.D12;
            DamageDie = damageDie;

            damageType ??= "bludgeoning";
            DamageType = damageType;

            Range = 5;
        }
    }
}