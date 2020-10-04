namespace DungeonCrawler
{
    public class Wall : IMappable
    {
        public Point Location {get; protected set;}
        public char Symbol {get; protected set;} = Symbols.Barrier;

        public Wall(Point location)
        {
            Location = location;
        }
        public void SetLocation(Point location)
        {   
            Location = location;
        }
    }
}