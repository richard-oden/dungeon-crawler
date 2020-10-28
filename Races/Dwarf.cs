using System.Collections.Generic;

namespace DungeonCrawler
{
    public class Dwarf : Race
    {
        public override IEntityAction Action {get; protected set;}
        public Dwarf(string abil1, string abil2)
        {
            Name = "Dwarf";
            HitDie = Dice.D8;
            NaturalAbilities = new List<string> {"STR", "CON", "WIS"};
            checkAndSetMods(abil1, abil2);
        }

        private bool endure()
        {
            // temp hp bonus
            return true;
        }
    }
}