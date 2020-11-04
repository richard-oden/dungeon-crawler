using System;
using System.Collections.Generic;
using System.Linq;
using static DungeonCrawler.ExtensionsAndHelpers;

namespace DungeonCrawler
{
    public class Combat
    {
        public List<Entity> Combatants {get; protected set;}

        public Combat(List<Entity> combatants)
        {
            foreach (Entity combatant in combatants) combatant.InitiativeRoll();
            combatants.Sort(new InitiativeComparer());
            Combatants = combatants;
        }

        public void StartCombat()
        {

            Console.WriteLine("Combat has begun! Current initiative order:");
            Console.WriteLine(GetInitiativeOrder());
            while (true)
            {
                foreach (Entity combatant in Combatants)
                {
                    if (!StillFighting()) 
                    {
                        break;
                    }
                    else if (combatant.IsDead)
                    {
                        Console.WriteLine($"{combatant.Name} cannot take their turn because {combatant.Pronouns[0].ToLower()} is dead.");
                        PressAnyKeyToContinue();
                    }
                    else 
                    {
                        Console.WriteLine($"It is {combatant.Name}'s turn.");
                        PressAnyKeyToContinue();
                        Console.Clear();
                        combatant.TakeTurn(this);
                    }
                }
            }
        }

        public string GetInitiativeOrder()
        {
            string output = "";
            for (int i = 0; i < Combatants.Count; i++) 
            {
                output += $"{i+1}. {Combatants[i].Name} ({Combatants[i].CurrentInitiative})\n";
            }
            return output;
        }

        public bool StillFighting()
        {
            // Check if only one team remains:
            var remainingCombatants = Combatants.Where(c => !c.IsDead);
            var remainingTeams = remainingCombatants.GroupBy(c => c.Team).ToList();
            if (remainingTeams.Count == 1)
            {
                var winner = remainingTeams[0].Select(c => c.Name).FormatToString("and");
                Console.WriteLine($"The fight is over! {winner} win(s)!");
                return false;
            }
            else if (Combatants.All(c => c.IsDead))
            {
                Console.WriteLine("The fight is over! All players have died.");
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}