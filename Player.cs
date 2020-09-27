namespace DungeonCrawler
{
    public class Player : SentientCreature
    {
        public Player(string name, int level, char gender, int[] abilityScoreValues, Race race, Caste caste) : base(name, level, gender, abilityScoreValues, race, caste)
        {
        }

        public override string GetDescription()
        {
            return $"{Name} is a level {Level.Value} {Race.Name} {Caste.Name}. {Pronouns[2]} hit die is a d{_hitDie.NumSides.Value}, and {Pronouns[2].ToLower()} total HP is {_hp}.";
        }
    }
}