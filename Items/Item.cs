using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace DungeonCrawler
{
    public class Item : IMappable, INamed, IDescribable
    {
        public string Name {get; protected set;}
        public double Value {get; protected set;}
        public double Weight {get; protected set;}
        public Dictionary<string, int> AbilityMods {get; protected set;}
        public MapPoint Location {get; protected set;}
        public char Symbol {get; protected set;} = Symbols.Item;
        public string Description {get; protected set;}

        public Item(string name, double value, double weight, Dictionary<string, int> abilityMods = null, MapPoint location = null)
        {
            Name = name;
            Value = value;
            Weight = weight;
            abilityMods ??= new Dictionary<string, int>();
            AbilityMods = abilityMods;
            Location = location;
        }

        public void SetLocation(MapPoint location)
        {   
            Location = location;
        }

        public void SetDescription(string description)
        {
            Description = description;
        }

        
        public static Dictionary<string, int> ToItemAbilityMods(string abilityMods)
        {
            var output = new Dictionary<string, int>();
            var abilityModsArray = abilityMods.Split(' ');
            for (var i = 0; i < abilityModsArray.Length; i += 2) 
            {
                output.Add(abilityModsArray[i], int.Parse(abilityModsArray[i+1]));
            }
            return output;
        }
        public static List<Item> ImportFromCsv(string csvFileName)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            var fileName = Path.Combine(currentDirectory, csvFileName);
            var Items = new List<Item>();
            using (var reader = new StreamReader(fileName)) // using directive closes streamreader after finished
            {
                string line = "";
                reader.ReadLine(); // consumes first line, which only contains headings
                while((line = reader.ReadLine()) != null) // while the line is not null
                {
                    string[] values = line.Split(',');
                    if (values[0] == "Item Type" || String.IsNullOrEmpty(values[0]))
                    {
                        continue;
                    }
                    Item item;
                    if (values[0] == "Weapon")
                    {
                        // object[] paramArray = values.Where(v => Array.IndexOf(values, v) != 0 &&
                        //                                     !String.IsNullOrEmpty(v))
                        //                             .
                        // Type weaponType = Type.GetType(values[1]);
                        // object weapon = Activator.CreateInstance(weaponType, new object[] 
                        // {
                        //     attackBonus: values[4], 
                        //     damageBonus: values[5], 
                        //     damageType: values[6]
                        // });
                    }
                    else if (values[0] == "Armor")
                    {
                        item = new Armor(slot: values[1],
                            name: values[2], 
                            value: Double.Parse(values[3]), 
                            weight: Double.Parse(values[4]), 
                            ac: Int32.Parse(values[6]),
                            material: values[5],
                            abilityMods: (String.IsNullOrEmpty(values[9]) ? null : ToItemAbilityMods(values[9])));
                    }
                    else if (values[0] == "Consumable")
                    {

                    }
                    else if (values[0] == "Misc")
                    {

                    }
                    else
                    {
                        throw new Exception($"Unexpected item type '{values[0]}'!");
                    }
                    Items.Add(item);
                }
            }
            return Items;
        }
    }
}