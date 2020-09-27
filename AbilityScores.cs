using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonCrawler
{
    public class AbilityScores
    {
        public Dictionary<string, Stat> BaseScores {get; private set;} = new Dictionary<string, Stat>()
        {
            {"STR", new Stat("Strength", 0, 20)},
            {"CON", new Stat("Constitution", 0, 20)},
            {"DEX", new Stat("Dexterity", 0, 20)},
            {"INT", new Stat("Intelligence", 0, 20)},
            {"WIS", new Stat("Wisdom", 0, 20)},
            {"CHA", new Stat("Charisma", 0, 20)}
        };
        public Dictionary<string, int> RacialMods {get; private set;} = new Dictionary<string, int>();
        public Dictionary<string, int> ItemMods {get; private set;} = new Dictionary<string, int>();
        public Dictionary<string, int> TempMods {get; private set;} = new Dictionary<string, int>();
        public Dictionary<string, int> TotalScores 
        {
            get
            {
                var output = new Dictionary<string, int>();
                foreach (var abil in BaseScores)
                {
                    int score = abil.Value.Value;
                    if (RacialMods.ContainsKey(abil.Key)) score += RacialMods[abil.Key];
                    if (ItemMods.ContainsKey(abil.Key)) score += ItemMods[abil.Key];
                    if (TempMods.ContainsKey(abil.Key)) score += TempMods[abil.Key];
                    output.Add(abil.Key, score);
                }
                return output;
            }
        }
        
        public AbilityScores()
        {
            foreach (var score in BaseScores) score.Value.SetValue(10);
            RacialMods = new Dictionary<string, int>();
            ItemMods = new Dictionary<string, int>();
            TempMods = new Dictionary<string, int>();
        }
        public AbilityScores(int[] scores)
        {
            int i = 0;
            foreach (var score in BaseScores)
            {
                score.Value.SetValue(scores[i]);
                i++;
            }
        }
        public AbilityScores(int[] scores, Dictionary<string, int> racialMods, Dictionary<string, int> itemMods,  Dictionary<string, int> tempMods) : this(scores)
        {
            SetMods(RacialMods, racialMods);
            SetMods(ItemMods, itemMods);
            SetMods(TempMods, tempMods);
        }
        
        //Take subset of mods to be applied, generate full set of modifiers:
        public void SetMods(Dictionary<string, int> currentMods, Dictionary<string, int> newModsSubset)
        {
            foreach (var abil in BaseScores)
            {
                int modifier = 0;
                if (newModsSubset.ContainsKey(abil.Key)) modifier += newModsSubset[abil.Key];

                // Add to modifiers if already generated:
                if (currentMods.ContainsKey(abil.Key))
                {
                    currentMods[abil.Key] += modifier;
                }
                else
                {
                    currentMods.Add(abil.Key, modifier);
                }
            }
        }

        public string GetDescription()
        {
            char plusOrMinus(int mod)
            {
                 return mod > 0 ? '+' : '-';
            }

            string output = "";
            foreach (var abil in BaseScores) 
            {
                // Base scores:
                output += $"{abil.Key}: {abil.Value.Value}";
                // Racial modifiers:
                output += $" {plusOrMinus(RacialMods[abil.Key])} {Math.Abs(RacialMods[abil.Key])}";
                // Item modifiers:
                output += $" {plusOrMinus(ItemMods[abil.Key])} {Math.Abs(ItemMods[abil.Key])}";
                // // Temp modifiers:
                output += $" {plusOrMinus(TempMods[abil.Key])} {Math.Abs(TempMods[abil.Key])}";
                // Total scores:
                output += $" = {TotalScores[abil.Key]}\n";
            }
            return output;
        }
    }
}