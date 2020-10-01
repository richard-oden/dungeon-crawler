using System;
using System.Collections.Generic;

namespace DungeonCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            var testHammer = new Warhammer
            (
                "Furious Hammer", 100.0, 36.4, false, 
                abilityMods: new Dictionary<string, int> {{"CON", 2}, {"WIS", -2}}
            );

            var testTome = new Tome
            (
                "Fire Tome", 160, 24, false,
                damageBonus: 4, damageDie: Dice.D20, damageType: "fire"
            );

            var player1 = new Player
            (
                "Stinthad", 10, 'm', 
                new int[]{18, 12, 16, 8, 8, 10},
                new Dwarf("STR", "CON"), new Fighter()
            );
            // Console.WriteLine(player1.GetDescription());

            var player2 = new Player
            (
                "Theodas", 10, 'm', 
                new int[]{8, 12, 10, 18, 12, 8},
                new Elf("INT", "WIS"), new Wizard()
            );
            // Console.WriteLine(player2.GetDescription());

            player1.AddItem(testHammer);
            player1.AddItem(testHammer);
            
            player2.AddItem(testTome);
            Console.WriteLine(player1.GetAllStats() + "\n");
            
            Console.WriteLine(player2.GetAllStats() + "\n");

            player1.DoAttack(player2);
            player1.DoAttack(player2);
            player1.DoAttack(player2);

            Console.WriteLine("\n" + player2.GetAllStats());
        }
    }
}
