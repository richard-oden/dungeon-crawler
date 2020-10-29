using System;
using System.Collections.Generic;

namespace DungeonCrawler
{
    public class Dwarf : Race
    {
        public override IEntityAction Action {get; protected set;}
        public Dwarf(string abil1, string abil2)
        {
            Name = "Dwarf";
            HitDie = Dice.D8;
            NaturalAbilities = new List<string> {"STR", "CON", "WIS"};
            checkAndSetMods(abil1, abil2);
            Action = new NonTargetedAction("bolster", "- Dwarf Ability: You bolster yourself, gaining 3d6+CON temp HP for 5 turns.", "major", bolster);
        }

        private bool bolster()
        {
            System.Console.WriteLine($"{SentientCreature.Name} is bolstering {SentientCreature.Pronouns[1].ToLower()}self!");
            var bolstered = new StatusEffect("bolstered", 5, "TempHp", Dice.D6.Roll(3, SentientCreature.GetModifier("CON"), true), undoWhenFinished: true);
            SentientCreature.AddStatusEffect(bolstered);
            return true;
        }
    }
}