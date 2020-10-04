namespace DungeonCrawler
{
    public class Player : SentientCreature
    {
        public override char Symbol {get; protected set;} = Symbols.PlayerS;
        public Player(string name, int level, char gender, int[] abilityScoreValues, Race race, Caste caste, Point location = null) : base(name, level, gender, abilityScoreValues, race, caste, location)
        {
        }

        public override string GetDescription()
        {
            return $"{Name} is a level {Level.Value} {Race.Name} {Caste.Name}. {Pronouns[2]} hit die is a d{_hitDie.NumSides.Value}, and {Pronouns[2].ToLower()} total HP is {_hp}.";
        }
    }
}