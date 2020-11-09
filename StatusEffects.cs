namespace DungeonCrawler
{
    public static class StatusEffects
    {
        public static StatusEffect HealingMinor => new StatusEffect("healing (minor)", 0, "CurrentHp", Dice.D4.Roll(3));
        public static StatusEffect HealingMajor => new StatusEffect("healing (major)", 0, "CurrentHp", Dice.D8.Roll(3));
        public static StatusEffect Toughened => new StatusEffect("toughened", 3, "_baseArmorClass", 5, undoWhenFinished: true);
        public static StatusEffect Invisible => new StatusEffect("invisible", 3, "HiddenDc", 20, undoWhenFinished: true);
        public static StatusEffect Quickened => new StatusEffect("quickened", 3, "AbilityScores", 4, "DEX", undoWhenFinished: true);
        public static StatusEffect Frostbitten => new StatusEffect("frostbitten", 2, "CurrentHp", -2, recurring: true);
        public static StatusEffect Slowed => new StatusEffect("slowed", 2, "AbilityScores", 2, "DEX", undoWhenFinished: true);
        public static StatusEffect OnFire => new StatusEffect("on fire", 2, "CurrentHp", -4, recurring: true);
        public static StatusEffect CoveredInAcid => new StatusEffect("covered in acid", 2, "CurrentHp", -4, recurring: true);
        public static StatusEffect Deafened => new StatusEffect("frostbitten", 2, "AbilityScores", -4, "WIS", undoWhenFinished: true);
        public static StatusEffect Bleeding => new StatusEffect("bleeding", 2, "CurrentHp", -2, recurring: true);
        public static StatusEffect Concussed => new StatusEffect("concussed", 2, "AbilityScores", -2, "DEX", undoWhenFinished: true);
        public static StatusEffect Weakened => new StatusEffect("weakened", 2, "AbilityScores", -2, "STR", undoWhenFinished: true);
    }
}