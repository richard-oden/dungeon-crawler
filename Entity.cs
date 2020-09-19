namespace DungeonCrawler
{
    public class Entity
    {
        public string Name {get; private set;}
        public Stat Level {get; private set;} = new Stat("Level", 1, 100);
        public int Experience {get; private set;}
        public AbilityScores AbilityScores {get; private set;}
        public Stat HP {get; private set;} = new Stat("Hit Points");
        public Entity(string name, Stat level, AbilityScores abilityScores)
        {
            Name = name;
            Level = level;
            AbilityScores = abilityScores;
        }
    }
}