using System.Collections.Generic;

namespace DungeonCrawler
{
    public abstract class Race
    {
        public string Name {get; protected set;}
        public Die HitDie {get; protected set;}
        public List<string> NaturalAbilities {get; protected set;}
    }
}