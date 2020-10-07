namespace DungeonCrawler
{
    public interface IMappable
    {
        MapPoint Location {get;}
        char Symbol {get;}
        void SetLocation(MapPoint Location);
    }
}