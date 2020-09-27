using System.Collections.Generic;

namespace DungeonCrawler
{
    public class Lizardfolk : Race
    {
        public Lizardfolk(string abil1, string abil2)
        {
            Name = "Lizardfolk";
            HitDie = Dice.D8;
            NaturalAbilities = new List<string> {"STR", "CON", "DEX"};
            checkAndSetMods(abil1, abil2);
        }
    }
}