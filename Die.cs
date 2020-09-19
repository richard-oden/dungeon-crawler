using System;

namespace DungeonCrawler
{
    public class Die
    {
        public Stat NumSides {get; private set;} = new Stat("Number of Sides", 2, 100);
        private static Random rand = new System.Random();

        public Die(int numSides)
        {
            NumSides.SetValue(numSides);
        }

        public int Roll()
        {
            return rand.Next(1, NumSides.Value);
        }

        public int Roll(int multiplier)
        {
            return multiplier * Roll();
        }

        public int Roll(int multiplier, int modifier)
        {
            return Roll(multiplier) + modifier;
        }
    }
}