using System;
using System.Collections.Generic;
using System.Linq;

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
            bool stillFighting = true;
            while (stillFighting)
            {
                var npcs = from c in Combatants where c is INpc select (INpc)c;
                var players = from c in Combatants where c is Player select (Player)c;
                bool alliesWin = npcs.Any(n => n.Disposition < (Disposition)1);
                bool enemiesWin = npcs.Any(n => n.Disposition > (Disposition)0) && !players.Any(p => !p.IsDead);
                if (alliesWin || enemiesWin) stillFighting = false;
                
                foreach (Entity combatant in Combatants)
                {
                    Console.WriteLine($"It is {combatant.Name}'s turn.");
                    combatant.TakeTurn();
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
    }
}