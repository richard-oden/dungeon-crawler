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
            bool fighting = true;
            while (fighting)
            {
                fighting = StillFighting();
                foreach (Entity combatant in Combatants)
                {
                    if (!StillFighting()) break;
                    Console.WriteLine($"It is {combatant.Name}'s turn.");
                    PressAnyKeyToContinue();
                    combatant.TakeTurn(this);
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
            if (Combatants.Where(c => !c.IsDead).All(c => c.Team == Combatants[0].Team))
            {
                var winner = Combatants.Where(c => !c.IsDead).Select(c => c.Name).FormatToString("and");
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