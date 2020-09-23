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
        protected Dictionary<string, Stat> _abilityScores = new Dictionary<string, Stat>()
        {
            {"STR", new Stat("Strength", 0, 20)},
            {"CON", new Stat("Constitution", 0, 20)},
            {"DEX", new Stat("Dexterity", 0, 20)},
            {"INT", new Stat("Intelligence", 0, 20)},
            {"WIS", new Stat("Wisdom", 0, 20)},
            {"CHA", new Stat("Charisma", 0, 20)}
        };
        protected Die _hitDie;
        protected int _hp;
        public Entity(string name, int level, char gender, int[] abilityScoreValues = null, Die hitDie = null)
        {
            Name = name;
            Level.SetValue(level);
            Gender = gender;

            abilityScoreValues ??= new int[] {10, 10, 10, 10, 10, 10};
            int i = 0;
            foreach (KeyValuePair<string, Stat> abilScore in _abilityScores)
            {
                abilScore.Value.SetValue(abilityScoreValues[i]);
                i++;
            }

            _experience = 0;
            hitDie ??= new Die(6);
            _hitDie = hitDie;
            _hp = 0;
            for (int j = 0; j < Level.Value; j++)
            {
                _hp += _hitDie.Roll(1, getModifier(_abilityScores["CON"]));
            }
        }

        protected int getModifier(Stat abilityScore)
        {
            return (int)Math.Floor(((double)abilityScore.Value - 10.0) / 2.0);
        }

        public int AbilityCheck(string abil)
        {
            if (_abilityScores.ContainsKey(abil))
            {
                Console.WriteLine($"Rolling {_abilityScores[abil].Name} check for {this.Name}...");
                return D20.Roll(1, getModifier(_abilityScores[abil]), true);
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
    }
}