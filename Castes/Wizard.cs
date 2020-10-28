using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonCrawler
{
    class Wizard : Caste
    {
        public override IEntityAction Action {get; protected set;}
        public Wizard()
        {
            Name = "Wizard";
            AbilityProficiency = "INT";
            ArmorProficiency = "cloth";
            CanUseShield = false;
            WeaponProficiency = new List<string> {"staff", "wand", "tome"};
            Action = new TargetedAction("slow", "[target name] - Attack a target. If they fail a DEX check vs your INT, their DEX is reduced by your INT mod for 2 turns.", "major", slow, true);
        }

        private bool slow(string targetName)
        {
            StatusEffect slowed = new StatusEffect("slowed", 2, "AbilityScores", SentientCreature.GetModifier("INT")*-1, "DEX", undoWhenFinished: true);
            if (SentientCreature.Attack(targetName))
            {
                var entitiesOnMap = from o in SentientCreature.Location.Map.Objects where o is Entity select (Entity)o;
                var target = entitiesOnMap.Where(o => o is Entity)
                            .FirstOrDefault(e => (e as Entity).Name.ToLower() == targetName.ToLower());
                Console.WriteLine($"{SentientCreature.Name} is attempting to slow {target.Name}!");
                int targetSavingThrow = Dice.D20.Roll(1, target.GetModifier("DEX"), true);
                int passiveIntelligence = SentientCreature.AbilityScores.TotalScores["INT"];
                if (targetSavingThrow < passiveIntelligence)
                {
                    target.ApplyStatusEffect(slowed);
                    Console.WriteLine($"{SentientCreature.Name} slowed {target.Name}! Their DEX is reduced by {SentientCreature.GetModifier("INT")} for 2 turns!");
                }
                else
                {
                    Console.WriteLine($"{target.Name} resisted the attempt at being slowed!");
                }
                return true;
            }
            return false;
        }
    }
}