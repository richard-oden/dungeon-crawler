using System;
using System.Collections.Generic;

namespace DungeonCrawler
{
    class Cleric : Caste
    {
        public override IEntityAction Action {get; protected set;}
        public Cleric()
        {
            Name = "Cleric";
            AbilityProficiency = "WIS";
            ArmorProficiency = "chain";
            CanUseShield = true;
            WeaponProficiency = new List<string> {"flail", "mace", "warhammer"};
            Action = new NonTargetedAction("pray", "- Cleric Ability: You pray to your god and regain 1d8+WIS hp.", "major", prayer);
        }

        private bool prayer()
        {
            Console.WriteLine($"{SentientCreature.Name} is saying a prayer...");
            int roll = Dice.D8.Roll(1, SentientCreature.GetModifier("WIS"), true);
            SentientCreature.CurrentHp.ChangeValue(roll);
            Console.WriteLine($"{SentientCreature.Pronouns[0]} gained {roll} hp!");
            return true;
        }
    }
}