using System.Collections.Generic;

namespace DungeonCrawler
{
    public class Elf : Race
    {
        public Elf(string abil1, string abil2)
        {
            Name = "Elf";
            HitDie = Dice.D6;
            NaturalAbilities = new List<string> {"DEX", "INT", "WIS"};
            checkAndSetMods(abil1, abil2);
        }
    }
}