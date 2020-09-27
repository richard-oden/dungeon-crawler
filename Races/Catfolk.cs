using System.Collections.Generic;

namespace DungeonCrawler
{
    public class Catfolk : Race
    {
        public Catfolk(string abil1, string abil2)
        {
            Name = "Catfolk";
            HitDie = Dice.D4;
            NaturalAbilities = new List<string> {"DEX", "WIS", "CHA"};
            checkAndSetMods(abil1, abil2);
        }
    }
}