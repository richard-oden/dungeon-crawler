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
        public string DescriptionText {get; protected set;}
        public string Description {get; protected set;}

        public Item(string name, double value, double weight, 
                    Dictionary<string, int> abilityMods = null, string descriptionText = null, MapPoint location = null)
        {
            Name = name;
            Value = value;
            Weight = weight;
            abilityMods ??= new Dictionary<string, int>();
            AbilityMods = abilityMods;
            DescriptionText = descriptionText;
            Description = DescriptionText + $" It weighs roughly {Weight}lbs and may be worth around {Value} gold.";
            if (AbilityMods != null)
            {
                string abilModsString = AbilityMods.Select(aM => $"{aM.Key} + {aM.Value}").FormatToString("and");
                descriptionText += $" It gives the following ability score bonuses when held: {abilModsString}.";
            }
            Location = location;
        }

        public void SetLocation(MapPoint location)
        {   
            Location = location;
        }

        private static Dictionary<string, int> parseItemAbilityMods(string abilityMods)
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
                    var baseItem = new Item(
                        name: values[2], 
                        value: Double.Parse(values[3]),
                        weight: Double.Parse(values[4]),
                        abilityMods: String.IsNullOrEmpty(values[8]) ? null : parseItemAbilityMods(values[8]),
                        descriptionText: values[9]
                    );

                    if (values[0] == "Weapon")
                    {
                        Items.Add(new Weapon(
                            baseItem: baseItem,
                            type: values[1].ToLower(),
                            attackBonus: String.IsNullOrEmpty(values[5]) ? 0 : int.Parse(values[5]),
                            damageBonus: String.IsNullOrEmpty(values[6]) ? 0 : int.Parse(values[6]),
                            damageType: values[7]
                        ));
                    }
                    else if (values[0] == "Armor")
                    {
                        Items.Add(new Armor(
                            baseItem: baseItem,
                            slot: values[1].ToLower(),
                            material: values[5],
                            ac: int.Parse(values[6])
                        ));
                    }
                    else if (values[0] == "Consumable")
                    {
                        Items.Add(new Consumable(
                            baseItem: baseItem,
                            StatusEffect.ParseStatusEffects(values[1])
                        ));
                    }
                    else if (values[0] == "Misc")
                    {
                        Items.Add(baseItem);
                    }
                    else
                    {
                        throw new Exception($"Unexpected item type '{values[0]}'!");
                    }
                }
            }
            return Items;
        }
    }
}