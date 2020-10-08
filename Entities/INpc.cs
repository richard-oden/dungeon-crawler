namespace DungeonCrawler
{
    public interface INpc
    {
        Disposition Disposition {get;}
    }

    public enum Disposition
    {
        Hostile,
        Neutral,
        Friendly,
        Allied
    }
}