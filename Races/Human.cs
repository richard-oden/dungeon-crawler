using System.Collections.Generic;

namespace DungeonCrawler
{
    public class Human : Race
    {
        public Human()
        {
            Name = "Human";
            HitDie = Dice.D6;
        }
    }
}