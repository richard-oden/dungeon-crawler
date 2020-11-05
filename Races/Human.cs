using System.Collections.Generic;


namespace DungeonCrawler
{
    public class Human : Race
    {
        public override IEntityAction Action {get; protected set;}
        public Human(string abil1, string abil2)
        {
            Name = "Human";
            HitDie = Dice.D6;
            AbilityMods = new Dictionary<string, int>() {{abil1, 2}, {abil2, 2}};
            checkAndSetMods(abil1, abil2);
            Action = new TargetedAction("adapt", "[ability] - Human Ability: You adapt to the situation, increasing your chosen ability score by 1d4 for 5 turns.", "major", adapt);
        }

        private bool adapt(string ability)
        {
            ability = ability.ToUpper();
            if (SentientCreature.AbilityScores.BaseScores.ContainsKey(ability))
            {
                var adapted = new StatusEffect("adapted", 5, "AbilityScores", Dice.D4.Roll(), ability, undoWhenFinished: true);
                System.Console.WriteLine($"{SentientCreature.Name} is adapting to the situation!");
                SentientCreature.AddStatusEffect(adapted);
                return true;
            }
            else
            {
                System.Console.WriteLine($"'{ability}' is not a valid ability score.");
            }
            return false;
        }
    }
}