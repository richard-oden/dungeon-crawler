using System;
using System.Collections.Generic;

namespace DungeonCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            var player1 = new Player("Richard", 5, 'm', new[]{8, 12, 10, 10, 12, 16}, new Elf("INT", "WIS"), new Cleric());

            var testArmor = new TorsoArmor("Raging Chestplate", 100, 50, 8, "plate", new Dictionary<string, int>() {{"WIS", -4}});

            Console.WriteLine(player1.AbilityScores.GetDescription());
            Console.WriteLine(player1.ArmorClass);

            player1.AddItem(testArmor);

            Console.WriteLine(player1.AbilityScores.GetDescription());
            Console.WriteLine(player1.ArmorClass);
        }
    }
}
