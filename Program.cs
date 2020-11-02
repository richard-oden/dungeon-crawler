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
            List<Item> demoItems = Item.ImportFromCsv("Items/DemoItems.csv");
            List<Entity> demoEntities = Entity.ImportFromCsv("Entities/DemoEntities.csv", demoItems);
            Map demoMap = Map.CsvToMap("Maps/CombatDemoMap.csv", demoItems, demoEntities);
            var combatants = (from o in demoMap.Objects where o is Entity select (Entity)o).ToList();

            var demoCombat = new Combat(combatants);
            demoCombat.StartCombat();
        }
    }
}