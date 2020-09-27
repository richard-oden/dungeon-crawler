using System;
using System.Collections.Generic;
using System.Linq;
using static DungeonCrawler.Dice;

namespace DungeonCrawler
{
    public abstract class SentientCreature : Entity
    {
        public Race Race {get; protected set;}
        public Caste Caste {get; protected set;}
        public Dictionary<string, Item> Items {get; protected set;}
        public SentientCreature(string name, int level, char gender, int[] abilityScoreValues, Race race, Caste caste) : base(name, level, gender, abilityScoreValues, null)
        {
            Race = race;
            Caste = caste;
            _hitDie = race.HitDie;
            AbilityScores.SetMods(AbilityScores.RacialMods, Race.AbilityMods);
            foreach (var item in Items) AbilityScores.SetMods(AbilityScores.ItemMods, item.Value.AbilityMods);
        }
        public override string GetDescription()
        {
            return $"{Name} is a level {Level.Value} {Race.Name} {Caste.Name}. {Pronouns[2]} hit die is a d{_hitDie.NumSides.Value}, and {Pronouns[2].ToLower()} total HP is {_hp}.";
        }
    }
}