using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonCrawler
{
    public class Elf : Race
    {
        public override IEntityAction Action {get; protected set;}
        public Elf(string abil1, string abil2)
        {
            Name = "Elf";
            HitDie = Dice.D6;
            NaturalAbilities = new List<string> {"DEX", "INT", "WIS"};
            checkAndSetMods(abil1, abil2);
            Action = new NonTargetedAction("entrance", "- Elf Ability: Enemies within 15 feet must make a WIS check vs your caste's ability proficiency. If they fail, their WIS is reduced by 1d4 for 5 turns.", "major", entrance);
        }

        private bool entrance()
        {
            var entranced = new StatusEffect("entranced", 5, "AbilityScores", Dice.D4.Roll()*-1, "WIS", undoWhenFinished: true);
            var entitiesOnMap = from o in SentientCreature.Location.Map.Objects where o is Entity select (Entity)o;
            var enemiesInRange = entitiesOnMap.Where(e => e.Team != SentientCreature.Team && 
                                    e.Location.InRangeOf(SentientCreature.Location, 3)).ToArray();
            if (enemiesInRange.Length > 0)
            {
                int passiveCasteAbility = SentientCreature.AbilityScores.TotalScores[SentientCreature.Caste.AbilityProficiency];
                foreach (var enemy in enemiesInRange)
                {
                    if (enemy.AbilityCheck("WIS") < passiveCasteAbility)
                    {
                        Console.WriteLine($"{enemy.Name} was entranced by {SentientCreature.Name}!");
                        enemy.AddStatusEffect(entranced);
                    }
                    else
                    {
                        Console.WriteLine($"{enemy.Name} avoided the attempt at being entranced by {SentientCreature.Name}!");
                    }
                }
                return true;
            }
            else
            {
                Console.WriteLine($"There are no enemies in range for {SentientCreature.Name} to entrance!");
            }
            return false;
        }
    }
}