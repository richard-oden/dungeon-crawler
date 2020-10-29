using System;
using System.Collections.Generic;

namespace DungeonCrawler
{
    public class Halfling : Race
    {
        public override IEntityAction Action {get; protected set;}
        public Halfling(string abil1, string abil2)
        {
            Name = "Halfling";
            HitDie = Dice.D4;
            NaturalAbilities = new List<string> {"DEX", "INT", "CHA"};
            checkAndSetMods(abil1, abil2);
            Action = new NonTargetedAction("dash", "- Halfling Ability: Your movement speed is increased by 15 feet for 5 turns.", "major", dash);
        }

        private bool dash()
        {
            var dashing = new StatusEffect("dashing", 5, "_baseMovementSpeedFeet", 15, undoWhenFinished: true);
            SentientCreature.AddStatusEffect(dashing);
            Console.WriteLine($"{SentientCreature.Name} is now dashing!");
            return true;
        }
    }
}