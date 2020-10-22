using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static DungeonCrawler.Symbols;

namespace DungeonCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.OutputEncoding = Encoding.UTF8;
            var map1 = new Map(10, 10);
            var testSword = new Sword
            (
                "Sword", 10, 10, true,
                damageType: "ice"
            );
            testSword.SetLocation(new MapPoint(6, 6, map1));

            // var testHammer = new Warhammer
            // (
            //     "Furious Hammer", 100.0, 36.4, false, 
            //     abilityMods: new Dictionary<string, int> {{"CON", 2}, {"WIS", -2}}
            // );

            var testTome = new Tome
            (
                "Fire Tome", 160, 24, damageType: "fire", damageBonus: 2
            );
            testTome.SetLocation(new MapPoint(2, 3, map1));

            var healthPotion1 = new Consumable
            (
                "Minor Health Potion", 30, 3, new List<StatusEffect>()
                {
                    new StatusEffect("healing", 0, "_currentHp", Dice.D4.Roll(3, true), false, false)
                }
            );

            var sentientNpc1 = new SentientNpc
            (
                name: "Theodas", level: 10, gender: 'm',
                race: new Elf("DEX", "INT"), caste: new Wizard(),
                abilityScoreValues: new []{8, 10, 10, 18, 14, 12},
                team: 0, aggression: Aggression.High,
                location: new MapPoint(5, 7, map1)
            );

            var giantRat1 = new NonSentientNpc
            (
                name: "Giant Rat", level: 5, gender: 'n',
                abilityScoreValues: new []{12, 14, 10, 4, 6, 10},
                team: 1, aggression: Aggression.High,
                location: new MapPoint(2, 3, map1)
            );

            var broodMother1 = new NonSentientNpc
            (
                name: "Rat Brood Mother", level: 7, gender: 'f',
                abilityScoreValues: new []{14, 12, 14, 4, 7, 11},
                team: 1, aggression: Aggression.High,
                location: new MapPoint(2, 4, map1)
            );

            var player1 = new Player
            (
                name: "Dyfena", level: 10, gender: 'f',
                race: new Elf("DEX", "WIS"), caste: new Cleric(),
                abilityScoreValues: new []{16, 14, 10, 10, 18, 8},
                location: new MapPoint(6, 5, map1)
            );
            var player2 = new Player
            (
                name: "Eldfar", level: 10, gender: 'm',
                race: new Elf("INT", "WIS"), caste: new Wizard(),
                abilityScoreValues: new []{8, 10, 10, 18, 12, 10},
                location: new MapPoint(4, 6, map1)
            );
            var player3 = new Player
            (
                name: "Stinthad", level: 10, gender: 'm',
                race: new Dwarf("STR", "CON"), caste: new Fighter(),
                abilityScoreValues: new []{18, 12, 16, 10, 18, 8},
                location: new MapPoint(6, 7, map1)
            );
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
                    player2,
                    player3,
                    testSword,
                    testTome,
                    broodMother1,
                    sentientNpc1     
                }
            );

            foreach (var obj in map1.Objects) 
            {
                if (obj is Entity)
                {
                    var entity = (Entity)obj;
                    entity.AddItem(healthPotion1);
                }
            }

            var combat1 = new Combat(new List<Entity> {player1, player2, player3, broodMother1, sentientNpc1});
            combat1.StartCombat();
        }
    }
}