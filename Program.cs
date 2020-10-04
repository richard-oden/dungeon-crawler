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
            var testSword = new Sword
            (
                "Frost Sword", 10, 10, true,
                damageType: "fire"
            );
            testSword.SetLocation(new Point(7, 8));

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
                new Point(5, 5)
            );
            // Console.WriteLine(player1.GetDescription());

            var sCreature1 = new SentientCreature
            (
                "Theodas", 10, 'm', 
                new int[]{8, 12, 10, 18, 12, 8},
                new Elf("INT", "WIS"), new Wizard(),
                new Point(2, 4)
            );
            // Console.WriteLine(player2.GetDescription());

            var map1 = new Map
            (
                10, 10, new List<IMappable> 
                {
                    new Wall(new Point(0, 0)),
                    new Wall(new Point(1, 0)),
                    new Wall(new Point(2, 0)),
                    new Wall(new Point(3, 0)),
                    new Wall(new Point(4, 0)),
                    new Wall(new Point(5, 0)),
                    new Wall(new Point(6, 0)),
                    new Wall(new Point(7, 0)),
                    new Wall(new Point(8, 0)),
                    new Wall(new Point(9, 0)),
                    new Wall(new Point(0, 1)),
                    new Wall(new Point(0, 2)),
                    new Wall(new Point(0, 3)),
                    new Wall(new Point(0, 4)),
                    new Wall(new Point(0, 5)),
                    new Wall(new Point(0, 6)),
                    new Wall(new Point(0, 7)),
                    new Wall(new Point(0, 8)),
                    new Wall(new Point(0, 9)),
                    new Wall(new Point(1, 9)),
                    new Wall(new Point(2, 9)),
                    new Wall(new Point(3, 9)),
                    new Wall(new Point(4, 9)),
                    new Wall(new Point(5, 9)),
                    new Wall(new Point(6, 9)),
                    new Wall(new Point(7, 9)),
                    new Door(new Point(8, 9)),
                    new Wall(new Point(9, 9)),
                    new Wall(new Point(9, 1)),
                    new Wall(new Point(9, 2)),
                    new Wall(new Point(9, 3)),
                    new Wall(new Point(9, 4)),
                    new Wall(new Point(9, 5)),
                    new Wall(new Point(9, 6)),
                    new Wall(new Point(9, 7)),
                    new Wall(new Point(9, 8)),
                    player1,
                    sCreature1,
                    testSword           
                }
            );
            map1.PrintMap();
        }
    }
}
