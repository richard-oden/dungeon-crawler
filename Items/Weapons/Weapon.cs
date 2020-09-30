using System.Collections.Generic;

namespace DungeonCrawler
{
    public abstract class Weapon : Item
    {
        public string Type {get; protected set;}
        public bool TwoHanded {get; protected set;}
        public int Range {get; protected set;}
        public Die DamageDie {get; protected set;}
        public string DamageType {get; protected set;}
        public int AttackBonus {get; protected set;}
        public int DamageBonus {get; protected set;}

        protected Weapon(string name, double value, double weight, bool twoHanded, Dictionary<string, int> abilityMods = null, int attackBonus = 0, int damageBonus = 0) : base(name, value, weight, abilityMods)
        {
            TwoHanded = twoHanded;
            AttackBonus = attackBonus;
            DamageBonus = damageBonus;
        }
    }
}