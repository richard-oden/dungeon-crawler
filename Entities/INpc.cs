using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonCrawler
{
    public interface INpc
    {
        Aggression Aggression {get;}
        List<Entity> KnownEntities {get;}
        List<Item> KnownItems {get;}

        void AttackedBy(Entity enemy);
        void DamagedBy(Entity enemy);
        void Prioritize();
    }

    public enum Aggression
    {
        Low,    // Will not fight back if attacked
        Mid,    // Will fight back if attacked
        High    // Will fight
    }
}