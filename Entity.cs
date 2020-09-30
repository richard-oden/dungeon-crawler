using System;
using System.Collections.Generic;
using static DungeonCrawler.Dice;

namespace DungeonCrawler
{
    public class Entity
    {
        public string Name {get; protected set;}
        public char Gender {get; protected set;}
        public string[] Pronouns
        {
            get
            {
                if (Gender == 'm') return new[] {"He", "Him", "His"};
                else if (Gender == 'f') return new[] {"She", "Her", "Her"};
                else if (Gender == 'n') return new[] {"They", "Them", "Their"};
                else return new [] {"It", "It", "Its"};
            }
        }
        public Stat Level {get; protected set;} = new Stat("Level", 1, 100);
        protected int _experience;
        public AbilityScores AbilityScores {get; protected set;}
        public virtual int ArmorClass => 10 + (AbilityScores.TotalScores["CON"] > AbilityScores.TotalScores["DEX"] ? getModifier("CON") : getModifier("DEX"));
        protected Die _hitDie;
        protected int _hp;
        protected double _maxCarryWeight => AbilityScores.TotalScores["STR"] * 15;
        protected double _currentWeightCarried
        {
            get
            {
                double totalWeight = 0;
                foreach (var i in Items) totalWeight += i.Weight;
                return totalWeight;
            }
        }
        public List<Item> Items {get; protected set;} = new List<Item>();
        public Entity(string name, int level, char gender, int[] abilityScoreValues = null, Die hitDie = null)
        {
            Name = name;
            Level.SetValue(level);
            Gender = gender;
            AbilityScores = new AbilityScores(abilityScoreValues);

            _experience = 0;
            hitDie ??= new Die(6);
            _hitDie = hitDie;
            _hp = 0;
            for (int j = 0; j < Level.Value; j++)
            {
                _hp += _hitDie.Roll(1, getModifier("CON"));
            }
        }

        protected int getModifier(string abilityScore)
        {
            return (int)Math.Floor(((double)AbilityScores.TotalScores[abilityScore] - 10.0) / 2.0);
        }

        public int AbilityCheck(string abil)
        {
            if (AbilityScores.BaseScores.ContainsKey(abil))
            {
                Console.WriteLine($"Rolling {AbilityScores.BaseScores[abil].Name} check for {this.Name}...");
                return D20.Roll(1, getModifier(abil), true);
            }
            else 
            {
                throw new InvalidAbilityException($"Ability '{abil}' in entity '{this.Name}' is not valid!");
            }
        }

        public virtual string GetDescription()
        {
            return $"{Name} lvl {Level.Value} has a hit die of d{_hitDie.NumSides.Value}, and total HP of {_hp}.";
        }

        public virtual void AddItem(Item newItem)
        {
            if (_currentWeightCarried + newItem.Weight <= _maxCarryWeight)
            {
                Items.Add(newItem);
            }
            else
            {
                Console.WriteLine($"{Name} cannot carry any more weight!");
            }
        }

        public void RemoveItem(Item heldItem)
        {
            if (Items.Contains(heldItem))
            {
                Items.Remove(heldItem);
            }
            else
            {
                string article = "AEIOUaeiou".IndexOf(heldItem.Name[0]) >= 0 ? "an" : "a";
                Console.WriteLine($"{Name} does not currently have {article} {heldItem.Name}.");
            }
        }

        public virtual int AttackRoll()
        {
            int mod = AbilityScores.TotalScores["CON"] > AbilityScores.TotalScores["DEX"] ? getModifier("CON") : getModifier("DEX");
            return Dice.D20.Roll(1, mod);
        }
    }
}