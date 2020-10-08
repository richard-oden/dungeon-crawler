namespace DungeonCrawler
{
    public class SentientNpc : SentientCreature, INpc
    {
        public Disposition Disposition {get; protected set;}
        public SentientNpc(string name, int level, char gender, Race race, Caste caste, int[] abilityScoreValues = null, MapPoint location = null, Disposition disposition = Disposition.Neutral) : base(name, level, gender, race, caste, abilityScoreValues, location)
        {
            Disposition = disposition;
        }
    }
}