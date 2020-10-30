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
            var dashing = new StatusEffect("dashing", 5, "_baseMovementSpeed", 15, undoWhenFinished: true);
            var dashingThisTurn = new StatusEffect("dashing this turn", 0, "_movementRemaining", 15);
            SentientCreature.AddStatusEffect(dashing);
            SentientCreature.AddStatusEffect(dashingThisTurn);
            Console.WriteLine($"{SentientCreature.Name} is now dashing!");
            return true;
        }
    }
}