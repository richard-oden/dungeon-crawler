namespace DungeonCrawler
{
    class ValueOutOfRangeException : System.Exception
    {
        public ValueOutOfRangeException()
        {}
        
        public ValueOutOfRangeException(string message) : base(message)
        {}
    }

    class InvalidAbilityException : System.Exception
    {
        public InvalidAbilityException()
        {}
        
        public InvalidAbilityException(string message) : base(message)
        {}
    }
}