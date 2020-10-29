using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonCrawler
{
    class Rogue : Caste
    {
        public override IEntityAction Action {get; protected set;}
        public Rogue()
        {
            Name = "Rogue";
            AbilityProficiency = "DEX";
            ArmorProficiency = "leather";
            CanUseShield = false;
            WeaponProficiency = new List<string> {"dagger", "bow", "sword"};
            Action = new TargetedAction("assassinate", "[target name] - Rogue Ability: Attack a target. If you are hidden to them and they fail a CON check vs your DEX, you deal an additional 4d6+DEX damage.", "major", assassinate);
        }

        private bool assassinate(string targetName)
        {
            if (SentientCreature.Attack(targetName))
            {
                var entitiesOnMap = from o in SentientCreature.Location.Map.Objects where o is Entity select (Entity)o;
                var target = entitiesOnMap.Where(o => o is Entity)
                            .FirstOrDefault(e => (e as Entity).Name.ToLower() == targetName.ToLower());
                Console.WriteLine($"{SentientCreature.Name} is attempting to assassinate {target.Name}!");
                if (SentientCreature.HiddenDc > target.PassivePerception)
                {
                    int passiveDexterity = SentientCreature.AbilityScores.TotalScores["DEX"];
                    if (target.AbilityCheck("CON") < passiveDexterity)
                    {
                        int damage = Dice.D6.Roll(4, SentientCreature.GetModifier("DEX"), true);
                        Console.WriteLine($"{SentientCreature.Name} struck {target.Name} from the shadows, dealing an additional {damage} points of damage!");
                        target.CurrentHp.ChangeValue(damage*-1);
                    }
                    else
                    {
                        Console.WriteLine($"{target.Name} resisted the assassination attempt!");
                    }
                }
                else
                {
                    Console.WriteLine($"{SentientCreature.Name} couldn't sneak up on {target.Name}!");
                }
                return true;
            }
            return false;
        }
    }
}