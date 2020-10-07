namespace DungeonCrawler
{
    public class Wall : IMappable
    {
        public MapPoint Location {get; protected set;}
        public char Symbol {get; protected set;} = Symbols.Barrier;

        public Wall(MapPoint location)
        {
            Location = location;
        }
        public void SetLocation(MapPoint location)
        {   
            Location = location;
        }
    }
}