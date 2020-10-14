using System;

namespace DungeonCrawler
{
    public interface IEntityAction
    {
        string Command {get;}
        string Description {get;}
        string Type {get;}
    }
}