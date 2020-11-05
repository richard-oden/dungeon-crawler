using System;
using System.Collections.Generic;

namespace DungeonCrawler
{
    public abstract class Race
    {
        public string Name {get; protected set;}
        public Die HitDie {get; protected set;}
        public List<string> NaturalAbilities {get; protected set;} = new List<string>();
        public Dictionary<string, int> AbilityMods {get; protected set;}
        public SentientCreature SentientCreature {get; set;}
        public virtual IEntityAction Action {get; protected set;}

        protected void checkAndSetMods(string abil1, string abil2)
        {
            if (this is Human)
            {
                AbilityMods = new Dictionary<string, int>() {{abil1, 1}, {abil2, 1}};
            }
            else if (NaturalAbilities.Contains(abil1) && NaturalAbilities.Contains(abil2))
            {
                AbilityMods = new Dictionary<string, int>() {{abil1, 2}, {abil2, 2}};
            }
            else
            {
                throw new InvalidAbilityException($"One or both of the abilities, {abil1} and {abil2} are not natural abilities for the race {this.Name}!");
            }
        }

        public static Race ParseRace(string race, string abilMods)
        {
            var abilModsArray = abilMods.Split(' ');
            if (race == "Human") return new Human(abilModsArray[0], abilModsArray[1]);
            else if (race == "Elf") return new Elf(abilModsArray[0], abilModsArray[1]);
            else if (race == "Dwarf") return new Dwarf(abilModsArray[0], abilModsArray[1]);
            else if (race == "Halfling") return new Halfling(abilModsArray[0], abilModsArray[1]);
            else throw new Exception($"'{race}' is not a valid race.");
        }
    }
}