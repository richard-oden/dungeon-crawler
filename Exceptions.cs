namespace DungeonCrawler
{
    class ValueOutOfRangeException : System.Exception
    {
        public ValueOutOfRangeException()
        {}
        
        public ValueOutOfRangeException(string message) : base(message)
        {}
    }
}