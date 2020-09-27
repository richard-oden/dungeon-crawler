using System.Collections.Generic;

namespace DungeonCrawler
{
    public class Human : Race
    {
        public Human(string abil1, string abil2)
        {
            Name = "Human";
            HitDie = Dice.D6;
            AbilityMods = new Dictionary<string, int>() {{abil1, 2}, {abil2, 2}};
            checkAndSetMods(abil1, abil2);
        }
    }
}