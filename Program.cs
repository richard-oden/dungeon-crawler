using System;
using System.Collections.Generic;
using System.Linq;
using static DungeonCrawler.Symbols;

namespace DungeonCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var map1 = new Map(10, 10);
            var testSword = new Sword
            (
                "Frost Sword", 10, 10, true,
                damageType: "fire"
            );
            testSword.SetLocation(new MapPoint(7, 9, map1));

            // var testHammer = new Warhammer
            // (
            //     "Furious Hammer", 100.0, 36.4, false, 
            //     abilityMods: new Dictionary<string, int> {{"CON", 2}, {"WIS", -2}}
            // );

            // var testTome = new Tome
            // (
            //     "Fire Tome", 160, 24, false,
            //     damageBonus: 4, damageDie: Dice.D20, damageType: "fire"
            // );

            var player1 = new Player
            (
                "Stinthad", 10, 'm', 
                new int[]{18, 12, 16, 8, 8, 10},
                new Dwarf("STR", "CON"), new Fighter(),
                new MapPoint(5, 5, map1)
            );
            // Console.WriteLine(player1.GetDescription());

            var sCreature1 = new SentientCreature
            (
                "Theodas", 10, 'm', 
                new int[]{8, 12, 10, 18, 12, 8},
                new Elf("INT", "WIS"), new Wizard(),
                new MapPoint(2, 4, map1)
            );
            // Console.WriteLine(player2.GetDescription());

            map1.AddObjects
            (
                new List<IMappable> 
                {
                    new Wall(new MapPoint(0, 0, map1)),
                    new Wall(new MapPoint(1, 0, map1)),
                    new Wall(new MapPoint(2, 0, map1)),
                    new Wall(new MapPoint(3, 0, map1)),
                    new Wall(new MapPoint(4, 0, map1)),
                    new Wall(new MapPoint(5, 0, map1)),
                    new Wall(new MapPoint(6, 0, map1)),
                    new Wall(new MapPoint(7, 0, map1)),
                    new Wall(new MapPoint(8, 0, map1)),
                    new Wall(new MapPoint(9, 0, map1)),
                    new Wall(new MapPoint(0, 1, map1)),
                    new Wall(new MapPoint(0, 2, map1)),
                    new Wall(new MapPoint(0, 3, map1)),
                    new Wall(new MapPoint(0, 4, map1)),
                    new Wall(new MapPoint(0, 5, map1)),
                    new Wall(new MapPoint(0, 6, map1)),
                    new Wall(new MapPoint(0, 7, map1)),
                    new Wall(new MapPoint(0, 8, map1)),
                    new Wall(new MapPoint(0, 9, map1)),
                    new Wall(new MapPoint(1, 9, map1)),
                    new Wall(new MapPoint(2, 9, map1)),
                    new Wall(new MapPoint(3, 9, map1)),
                    new Wall(new MapPoint(4, 9, map1)),
                    new Wall(new MapPoint(5, 9, map1)),
                    new Wall(new MapPoint(6, 9, map1)),
                    new Wall(new MapPoint(7, 9, map1)),
                    new Door(new MapPoint(8, 9, map1)),
                    new Wall(new MapPoint(9, 9, map1)),
                    new Wall(new MapPoint(9, 1, map1)),
                    new Wall(new MapPoint(9, 2, map1)),
                    new Wall(new MapPoint(9, 3, map1)),
                    new Wall(new MapPoint(9, 4, map1)),
                    new Wall(new MapPoint(9, 5, map1)),
                    new Wall(new MapPoint(9, 6, map1)),
                    new Wall(new MapPoint(9, 7, map1)),
                    new Wall(new MapPoint(9, 8, map1)),
                    player1,
                    sCreature1         
                }
            );
            // Movement input loop:
            while (true)
            {
                map1.PrintMap();
                string direction = Console.ReadLine();
                if (direction == "q") break;
                player1.Move(direction, 5);
                Console.Clear();
            }
        }
    }
}
