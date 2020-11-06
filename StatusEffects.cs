namespace DungeonCrawler
{
    public static class StatusEffects
    {
        public static StatusEffect HealingMinor => new StatusEffect("healing (minor)", 0, "CurrentHp", Dice.D4.Roll(3));
        public static StatusEffect HealingMajor => new StatusEffect("healing (major)", 0, "CurrentHp", Dice.D8.Roll(3));
        public static StatusEffect Toughened => new StatusEffect("toughened", 3, "_baseArmorClass", 5, undoWhenFinished: true);
        public static StatusEffect Invisible => new StatusEffect("invisible", 3, "HiddenDc", 20, undoWhenFinished: true);
        public static StatusEffect Quickened => new StatusEffect("quickened", 3, "AbilityScores", 4, "DEX", undoWhenFinished: true);
    }
}