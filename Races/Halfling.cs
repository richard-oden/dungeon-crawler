using System.Collections.Generic;

namespace DungeonCrawler
{
    public class Halfling : Race
    {
        public Halfling(string abil1, string abil2)
        {
            Name = "Halfling";
            HitDie = Dice.D4;
            NaturalAbilities = new List<string> {"DEX", "INT", "CHA"};
            checkAndSetMods(abil1, abil2);
        }
    }
}