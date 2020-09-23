using System.Collections.Generic;

namespace DungeonCrawler
{
    public class Lizardfolk : Race
    {
        public Lizardfolk()
        {
            Name = "Lizardfolk";
            HitDie = Dice.D8;
            NaturalAbilities = new List<string> {"STR", "CON", "DEX"};
        }
    }
}