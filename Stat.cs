using System;

namespace DungeonCrawler
{
    public class Stat
    {
        public string Name {get; private set;}
        public int MaxValue {get; private set;}
        public int MinValue {get; private set;}
        public int Value {get; private set;}

        public Stat(string name)
        {
            Name = name;
        }

        public Stat(string name, int minValue, int maxValue) : this(name)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public Stat(string name, int minValue, int maxValue, int value) : this(name, minValue, maxValue)
        {
            SetValue(value);
        }

        private bool inBounds(int value)
        {
            Console.WriteLine("Bounds check: " + (value > MinValue && value < MaxValue));
            return value > MinValue && value < MaxValue;
        }

        public void SetValue(int potentialValue)
        {
            if (inBounds(potentialValue)) 
            {
                Console.WriteLine("Potential Value: " + potentialValue);
                Value = potentialValue;
                Console.WriteLine("Current Value: " + Value);
            }
            else if (potentialValue > MaxValue) 
            {
                Console.WriteLine($"New value '{potentialValue}' for {Name} cannot be greater than {MaxValue}.");
                Value = MaxValue;
            }
            else if (potentialValue < MinValue)
            {
                Console.WriteLine($"New value '{potentialValue}' for {Name} cannot be lesser than {MinValue}.");
                Value = MinValue;
            }
        }

        public void ChangeValue(int amount)
        {
            int potentialValue = Value + amount;
            SetValue(potentialValue);
        }
    }
}