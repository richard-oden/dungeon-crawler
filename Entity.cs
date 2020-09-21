using System;

namespace DungeonCrawler
{
    public class Entity
    {
        public string Name {get; private set;}
        public Stat Level {get; private set;} = new Stat("Level", 1, 100);
        public int Experience {get; private set;}
        public AbilityScores AbilityScores {get; private set;}
        public Die HitDie {get; private set;}
        public int HP {get; private set;}
        public Entity(string name, int level, AbilityScores abilityScores)
        {
            Name = name;
            Level.SetValue(level);
            AbilityScores = abilityScores;
            Experience = 0;
            HitDie = new Die(6);
            HP = HitDie.Roll(Level.Value, GetModifier(AbilityScores.CON));
        }

        public Entity(string name, int level, AbilityScores abilityScores, Die hitDie) : this(name, level, abilityScores)
        {
            HitDie = hitDie;
        }

        public int GetModifier(Stat abilityScore)
        {
            return (int)Math.Floor(((double)abilityScore.Value - 10.0) / 2.0);
        }
    }
}