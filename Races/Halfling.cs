using System.Collections.Generic;

namespace DungeonCrawler
{
    public class Halfling : Race
    {
        public Halfling()
        {
            Name = "Halfling";
            HitDie = Dice.D4;
            NaturalAbilities = new List<string> {"DEX", "INT", "CHA"};
        }
    }
}