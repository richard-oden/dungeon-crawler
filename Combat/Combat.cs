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
            // Establish base knowledge of map objects for each npc: 
            foreach (Entity combatant in Combatants)
            {
                if (combatant is INpc) combatant.Search();
            }

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
            var npcs = from c in Combatants where c is INpc select (INpc)c;
            var players = from c in Combatants where c is Player select (Player)c;
            // Check combat ending conditions
            if (Combatants.Where(c => c.Team != 0).All(c => c.IsDead))
            {
                Console.WriteLine("The fight is over! No enemies remain.");
                return false;
            }
            else if (Combatants.Where(c => c is INpc && c.Team != 0 && !c.IsDead)
                                .All(c => (c as INpc).Aggression < Aggression.High))
            {
                Console.WriteLine("The fight is over! No hostile enemies remain.");
                return false;
            }
            else if (players.All(p => p.IsDead))
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