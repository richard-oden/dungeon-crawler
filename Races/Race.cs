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
            string article = "AEIOU".IndexOf(Name[0]) >= 0 ? "An" : "A";
            string abilities = "";
            for (int i = 0; i < NaturalAbilities.Count; i++)
            {
                abilities += NaturalAbilities[i];
                if (i != NaturalAbilities.Count - 1) abilities += ", ";
                if (i == NaturalAbilities.Count - 2) abilities += "or ";
            }
            return $"{article} {Name} may have better than average {abilities}, and its hit die is a d{HitDie.NumSides.Value}.";
        }
    }
}