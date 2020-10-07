namespace DungeonCrawler
{
    public class Door : IMappable
    {
        public MapPoint Location {get; protected set;}
        public char Symbol {get; protected set;} = Symbols.Door;

        public Door(MapPoint location)
        {
            Location = location;
        }

        public void SetLocation(MapPoint location)
        {   
            Location = location;
        }
    }
}