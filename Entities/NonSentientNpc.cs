namespace DungeonCrawler
{
    public class NonSentientNpc : Entity, INpc
    {
        public Aggression Aggression {get; protected set;}
        public NonSentientNpc(string name, int level, char gender, int team, Aggression aggression, int[] abilityScoreValues = null, Die hitDie = null, MapPoint location = null) : 
            base(name, level, gender, team, abilityScoreValues, hitDie, location)
        {
            Aggression = aggression;
        }
    }
}