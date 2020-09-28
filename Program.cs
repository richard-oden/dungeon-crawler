using System;
using System.Collections.Generic;

namespace DungeonCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            var player1 = new Player("Richard", 5, 'm', new[]{8, 12, 10, 10, 12, 16}, new Elf("INT", "WIS"), new Cleric());

            var testArmor = new TorsoArmor("testArmor", 100, 50, 6, "cloth", new Dictionary<string, int>() {{"WIS", 4}, {"CON", 2}});

            Console.WriteLine(player1.AbilityScores.GetDescription());

            player1.AddItem(testArmor);

            Console.WriteLine(player1.AbilityScores.GetDescription());
        }
    }
}
