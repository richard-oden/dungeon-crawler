using System.Collections.Generic;

namespace DungeonCrawler
{
    public class Elf : Race
    {
        public Elf()
        {
            Name = "Elf";
            HitDie = Dice.D6;
            NaturalAbilities = new List<string> {"DEX", "INT", "WIS"};
        }
    }
}