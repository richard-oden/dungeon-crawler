namespace DungeonCrawler
{
    public interface IMappable
    {
        Point Location {get;}
        char Symbol {get;}
        void SetLocation(Point Location);
    }
}