using System;

namespace DungeonCrawler
{
    public class Die
    {
        public Stat NumSides {get; private set;} = new Stat("Number of Sides", 2, 100);
        private static Random _rand = new System.Random();

        public Die(int numSides)
        {
            NumSides.SetValue(numSides);
        }

        public int Roll()
        {
            return _rand.Next(1, NumSides.Value+1);
        }

        public int Roll(int multiplier)
        {
            int total = 0;
            for (int i = 0; i < multiplier; i++) total += Roll();
            return total;
        }

        public int Roll(int multiplier, bool printOutput)
        {
            string output = $"Rolling {multiplier}d{NumSides.Value}: ";
            int total = 0;
            for (int i = 0; i < multiplier; i++) 
            {   
                int thisRoll = Roll();
                output += thisRoll + ((i == multiplier-1) ? " = " : " + ");
                total += thisRoll;
            }
            if (printOutput)
            {
                output += total;
                Console.WriteLine(output);
            }
            return total;
        }

        public int Roll(int multiplier, int modifier)
        {
            return Roll(multiplier) + modifier;
        }

        public int Roll(int multiplier, int modifier, bool printOutput)
        {
            string output = $"Rolling {multiplier}d{NumSides.Value}+{modifier}: ";
            int total = 0;
            for (int i = 0; i < multiplier; i++) 
            {   
                int thisRoll = Roll();
                output += $"{thisRoll} + ";
                total += thisRoll;
            }
            output += $"{modifier} = ";
            if (printOutput)
            {
                output += total;
                Console.WriteLine(output);
            }
            return total;
        }
    }
}