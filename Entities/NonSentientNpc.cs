namespace DungeonCrawler
{
    public class NonSentientNpc : Entity, INpc
    {
        public Disposition Disposition {get; protected set;}
        public NonSentientNpc(string name, int level, char gender, int[] abilityScoreValues = null, Die hitDie = null, MapPoint location = null, Disposition disposition = Disposition.Neutral) : base(name, level, gender, abilityScoreValues, hitDie, location)
        {
            Disposition = disposition;
        }
    }
}