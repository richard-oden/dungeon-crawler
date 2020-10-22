namespace DungeonCrawler
{
    public interface INpc
    {
        Aggression Aggression {get;}
        
    }

    public enum Aggression
    {
        Low,    // Will not fight back if attacked
        Mid,    // Will fight back if attacked
        High    // Will fight
    }
}