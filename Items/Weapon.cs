using System;
using System.Collections.Generic;

namespace DungeonCrawler
{
    public class Weapon : Item
    {
        public string Type {get; protected set;}
        public bool TwoHanded {get; protected set;}
        public int Range {get; protected set;}
        public Die DamageDie {get; protected set;}
        public string DamageType {get; protected set;}
        public int AttackBonus {get; protected set;}
        public int DamageBonus {get; protected set;}

        
        public Weapon(string type, string name, double value, double weight, Die damageDie = null, string damageType = null, 
            Dictionary<string, int> abilityMods = null, int attackBonus = 0, int damageBonus = 0, string descriptionText = null) : 
                base(name, value, weight, abilityMods, descriptionText)
        {
            if (type == "axe")
            {
                DamageDie = Dice.D10;
                DamageType = "slashing";
                Range = 5;
                TwoHanded = true;
            }
            else if (type == "bow")
            {
                DamageDie = Dice.D8;
                DamageType = "piercing";
                Range = 80;
                TwoHanded = true;
            }
            else if (type == "dagger")
            {
                DamageDie = Dice.D6;
                DamageType = "slashing";
                Range = 5;
                TwoHanded = false;
            }
            else if (type == "mace")
            {
                DamageDie = Dice.D10;
                DamageType = "bludgeoning";
                Range = 5;
                TwoHanded = true;
            }
            else if (type == "staff")
            {
                DamageDie = Dice.D10;
                DamageType = "magic";
                Range = 100;
                TwoHanded = true;
            }
            else if (type == "sword")
            {
                DamageDie = Dice.D8;
                DamageType = "slashing";
                Range = 5;
                TwoHanded = true;
            }
            else if (type == "tome")
            {
                DamageDie = Dice.D12;
                DamageType = "magic";
                Range = 60;
                TwoHanded = true;
            }
            else if (type == "warhammer")
            {
                DamageDie = Dice.D12;
                DamageType = "bludgeoning";
                Range = 5;
                TwoHanded = true;
            }
            else
            {
                throw new Exception($"Invalid weapon type '{type}' encountered!");
            }

            Type = type;
            AttackBonus = attackBonus;
            DamageBonus = damageBonus;
            DamageDie ??= damageDie;
            DamageType ??= damageType;
            Description += $" It deals 1d{DamageDie.NumSides.Value} {DamageType} damage and seems to have a range of {Range}ft.";
            if (AttackBonus != 0) Description += $" It grants + {AttackBonus} to attack rolls.";
            if (DamageBonus != 0) Description += $" It grants + {DamageBonus} to damage rolls.";
        }

        public Weapon(Item baseItem, string type, Die damageDie = null, string damageType = null, int attackBonus = 0, int damageBonus = 0) :
                this(type, baseItem.Name, baseItem.Value, baseItem.Weight, damageDie, damageType, baseItem.AbilityMods, attackBonus, damageBonus, baseItem.DescriptionText)
        {}
    }
}