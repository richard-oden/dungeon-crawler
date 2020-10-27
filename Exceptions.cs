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

    class InvalidEntityPropertyException : System.Exception
    {
        public InvalidEntityPropertyException()
        {}
        
        public InvalidEntityPropertyException(string message) : base(message)
        {}
    }

    class InvalidAggressionException : System.Exception
    {
        public InvalidAggressionException()
        {}
        
        public InvalidAggressionException(string message) : base(message)
        {}
    }

    class InvalidDirectionException : System.Exception
    {
        public InvalidDirectionException()
        {}
        
        public InvalidDirectionException(string message) : base(message)
        {}
    }

    class OutOfMapBoundsException : System.Exception
    {
        public OutOfMapBoundsException()
        {}
        
        public OutOfMapBoundsException(string message) : base(message)
        {}
    }
    
    class DuplicateLocationException : System.Exception
    {
        public DuplicateLocationException()
        {}
        
        public DuplicateLocationException(string message) : base(message)
        {}
    }
}