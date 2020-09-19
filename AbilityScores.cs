namespace DungeonCrawler
{
    public class AbilityScores
    {
        public Stat STR {get; private set;} = new Stat("Strength", 0, 20);
        public Stat CON {get; private set;} = new Stat("Constitution", 0, 20);
        public Stat DEX {get; private set;} = new Stat("Agility", 0, 20);
        public Stat INT {get; private set;} = new Stat("Intelligence", 0, 20);
        public Stat WIS {get; private set;} = new Stat("Wisdom", 0, 20);
        public Stat CHA {get; private set;} = new Stat("Charisma", 0, 20);

        public AbilityScores()
        {}

        public AbilityScores(int str, int con, int dex, int intel, int wis, int cha)
        {
            STR.SetValue(str);
            CON.SetValue(con);
            DEX.SetValue(dex);
            INT.SetValue(intel);
            WIS.SetValue(wis);
            CHA.SetValue(cha);
        }
    }
}