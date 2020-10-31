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

        
        protected Weapon(string type, string name, double value, double weight, 
            bool twoHanded = true, Die damageDie = null, string damageType = null, 
            Dictionary<string, int> abilityMods = null, int attackBonus = 0, int damageBonus = 0) : 
                base(name, value, weight, abilityMods)
        {
            Type = type;
            TwoHanded = twoHanded;
            AttackBonus = attackBonus;
            DamageBonus = damageBonus;
            DamageType = damageType;

            if (type == "axe")
            {
                damageDie ??= Dice.D10;
                DamageDie = damageDie;
                damageType ??= "slashing";
                Range = 8;
            }
            else if (Type == "bow")
            {
                damageDie ??= Dice.D8;
                DamageDie = damageDie;
                damageType ??= "piercing";
                Range = 80;
            }
            else if (Type == "dagger")
            {
                damageDie ??= Dice.D6;
                DamageDie = damageDie;
                damageType ??= "slashing";
                Range = 8;
            }
            else if (Type == "mace")
            {
                damageDie ??= Dice.D10;
                DamageDie = damageDie;
                damageType ??= "bludgeoning";
                Range = 8;
            }
            else if (Type == "staff")
            {
                damageDie ??= Dice.D10;
                DamageDie = damageDie;
                damageType ??= "magic";
                Range = 100;
            }
            else if (Type == "sword")
            {
                damageDie ??= Dice.D8;
                DamageDie = damageDie;
                damageType ??= "slashing";
                DamageType = damageType;
                Range = 8;
            }
            else if (Type == "tome")
            {
                damageDie ??= Dice.D12;
                DamageDie = damageDie;
                damageType ??= "magic";
                Range = 60;
            }
            else if (Type == "warhammer")
            {
                damageDie ??= Dice.D12;
                DamageDie = damageDie;
                damageType ??= "bludgeoning";
                Range = 8;
            }
        }
    }
}