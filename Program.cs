using System;

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

            var player1 = new Player("Richard", 5, 'm', new[]{8, 12, 10, 10, 12, 16}, new Dwarf(), new Cleric());

            Console.WriteLine(player1.GetDescription());
            Console.WriteLine(player1.Caste.GetDescription());
            player1.AbilityCheck("WIS");
        }
    }
}
