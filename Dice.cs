namespace DungeonCrawler
{
    public static class Dice
    {
        public static Die Coin {get; private set;} = new Die(2);
        public static Die D4 {get; private set;} = new Die(4);
        public static Die D6 {get; private set;} = new Die(6);
        public static Die D8 {get; private set;} = new Die(8);
        public static Die D10 {get; private set;} = new Die(10);
        public static Die D12 {get; private set;} = new Die(12);
        public static Die D20 {get; private set;} = new Die(20);
        public static Die D100 {get; private set;} = new Die(100);
    }
}