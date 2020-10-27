﻿using System;
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
            string title = @"
 ______   __   __  __    _  _______  _______  _______  __    _   
|      | |  | |  ||  |  | ||       ||       ||       ||  |  | |  
|  _    ||  | |  ||   |_| ||    ___||    ___||   _   ||   |_| |  
| | |   ||  |_|  ||       ||   | __ |   |___ |  | |  ||       |  
| |_|   ||       ||  _    ||   ||  ||    ___||  |_|  ||  _    |  
|       ||       || | |   ||   |_| ||   |___ |       || | |   |  
|______| |_______||_|  |__||_______||_______||_______||_|  |__|  
 _______  ______    _______  _     _  ___      _______  ______   
|       ||    _ |  |   _   || | _ | ||   |    |       ||    _ |  
|       ||   | ||  |  |_|  || || || ||   |    |    ___||   | ||  
|       ||   |_||_ |       ||       ||   |    |   |___ |   |_||_ 
|      _||    __  ||       ||       ||   |___ |    ___||    __  |
|     |_ |   |  | ||   _   ||   _   ||       ||   |___ |   |  | |
|_______||___|  |_||__| |__||__| |__||_______||_______||___|  |_|
-------------------- MULTIPLAYER COMBAT DEMO --------------------
";
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(title);
            Console.WriteLine("A game created by Richard Oden for Code Louisville's September 2020 C# class.\n");
            ExtensionsAndHelpers.PressAnyKeyToContinue();
            Console.Clear();

            Console.ResetColor();
            Console.OutputEncoding = Encoding.UTF8;
            Map demoMap = Map.CsvToMap("Maps/CombatDemoMap.csv");
            // var testSword = new Sword
            // (
            //     "Sword", 10, 10, true,
            //     damageType: "ice"
            // );

            // var testHammer = new Warhammer
            // (
            //     "Furious Hammer", 100.0, 36.4, false, 
            //     abilityMods: new Dictionary<string, int> {{"CON", 2}, {"WIS", -2}}
            // );

            // var testTome = new Tome
            // (
            //     "Fire Tome", 160, 24, damageType: "fire", damageBonus: 2
            // );

            var healthPotion1 = new Consumable
            (
                "Minor Health Potion", 30, 3, new List<StatusEffect>()
                {
                    new StatusEffect("healing", 0, "CurrentHp", Dice.D4.Roll(3), false, false)
                }
            );

            var player1 = new Player
            (
                name: "Dyfena", level: 10, gender: 'f',
                race: new Elf("DEX", "WIS"), caste: new Cleric(),
                abilityScoreValues: new []{16, 14, 10, 10, 18, 8},
                location: new MapPoint(3, 3, demoMap), team: 0
            );
            var player2 = new Player
            (
                name: "Eldfar", level: 10, gender: 'm',
                race: new Elf("INT", "WIS"), caste: new Wizard(),
                abilityScoreValues: new []{8, 10, 10, 18, 12, 10},
                location: new MapPoint(3, 18, demoMap), team: 1
            );
            var player3 = new Player
            (
                name: "Stinthad", level: 10, gender: 'm',
                race: new Dwarf("STR", "CON"), caste: new Fighter(),
                abilityScoreValues: new []{18, 12, 16, 10, 18, 8},
                location: new MapPoint(39, 3, demoMap), team: 2
            );

            demoMap.AddObjects(new List<IMappable>(){player1, player2, player3});

            foreach (var obj in demoMap.Objects) 
            {
                if (obj is Entity)
                {
                    var entity = (Entity)obj;
                    entity.AddItem(healthPotion1);
                }
            }

            var combat1 = new Combat(new List<Entity> {player1, player2, player3});
            combat1.StartCombat();
        }
    }
}