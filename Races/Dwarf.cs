using System.Collections.Generic;

namespace DungeonCrawler
{
    public class Dwarf : Race
    {
        public Dwarf()
        {
            Name = "Dwarf";
            HitDie = Dice.D8;
            NaturalAbilities = new List<string> {"STR", "CON", "WIS"};
        }
    }
}