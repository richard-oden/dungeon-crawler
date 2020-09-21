using System;

namespace DungeonCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            var testEntity = new Entity
            (
                "Test",
                1,
                new AbilityScores(16, 12, 9, 6, 8, 8),
                new Die(8)
            );

            // Console.WriteLine(testEntity.HitDie.Roll(1, 0));
            // Console.WriteLine(testEntity.AbilityScores.STR.Value);
            // testEntity.Level.SetValue(2);
            // Console.WriteLine($"{testEntity.Name}, lvl {testEntity.Level.Value} has a hit die of d{testEntity.HitDie.NumSides.Value}, and total HP of {testEntity.HP}.");
            testEntity.HitDie.Roll(10, 1, true);
        }
    }
}
