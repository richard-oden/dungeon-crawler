using System.Collections.Generic;

namespace DungeonCrawler
{
    public class Catfolk : Race
    {
        public Catfolk()
        {
            Name = "Catfolk";
            HitDie = Dice.D4;
            NaturalAbilities = new List<string> {"DEX", "WIS", "CHA"};
        }
    }
}