namespace DungeonCrawler
{
    public class SentientNpc : SentientCreature, INpc
    {
        public Aggression Aggression {get; protected set;}

        public SentientNpc(string name, int level, char gender, Race race, Caste caste, int team, Aggression aggression, int[] abilityScoreValues = null, MapPoint location = null) : 
            base(name, level, gender, race, caste, team, abilityScoreValues, location)
        {
            Aggression = aggression;
        }
    }
}