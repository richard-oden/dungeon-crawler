using System;
using System.Collections.Generic;

namespace DungeonCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            // var testEntity = new Entity
            // (
            //     "Test",
            //     10,
            //     'c',
            //     new[]{10, 6, 10, 10, 10, 10}
            // );
            // Console.WriteLine(testEntity.GetDescription());

            var player1 = new Player("Richard", 5, 'm', new[]{8, 12, 10, 10, 12, 16}, new Elf("INT", "WIS"), new Cleric());

            player1.AbilityScores.SetMods(player1.AbilityScores.ItemMods, new Dictionary<string, int>() {{"CON", 2}, {"CHA", 4}});
            player1.AbilityScores.SetMods(player1.AbilityScores.TempMods, new Dictionary<string, int>() {{"STR", -2}, {"DEX", -3}});

            // // Console.WriteLine(player1.GetDescription());
            // // Console.WriteLine(player1.Race.GetDescription());
            // // Console.WriteLine(player1.Caste.GetDescription());
            Console.WriteLine(player1.AbilityScores.GetDescription());
            Console.WriteLine(player1.ArmorClass);
        }
    }
}
