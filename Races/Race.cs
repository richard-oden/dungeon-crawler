using System.Collections.Generic;
using System.Linq;

namespace DungeonCrawler
{
    public abstract class Race
    {
        public string Name {get; protected set;}
        public Die HitDie {get; protected set;}
        public List<string> NaturalAbilities {get; protected set;} = new List<string>();
        public Dictionary<string, int> AbilityMods {get; protected set;}
        public SentientCreature SentientCreature {get; set;}
        public IEntityAction Action {get; protected set;}

        protected void checkAndSetMods(string abil1, string abil2)
        {
            if (NaturalAbilities.Contains(abil1) && NaturalAbilities.Contains(abil2))
            {
                AbilityMods = new Dictionary<string, int>() {{abil1, 2}, {abil2, 2}};
            }
            else
            {
                throw new InvalidAbilityException($"One or both of the abilities, {abil1} and {abil2} are not natural abilities for the race {this.Name}!");
            }
        }

        public string GetDescription()
        {
            return $"{Name.IndefiniteArticle()} {Name} may have better than average {NaturalAbilities.FormatToString("or")}, and its hit die is a d{HitDie.NumSides.Value}.";
        }
    }
}