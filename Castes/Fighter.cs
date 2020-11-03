using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonCrawler
{
    class Fighter : Caste
    {
        public override IEntityAction Action {get; protected set;}
        public Fighter()
        {
            Name = "Fighter";
            AbilityProficiency = "STR";
            ArmorProficiency = "plate";
            CanUseShield = true;
            WeaponProficiency = new List<string> {"sword", "axe", "warhammer"};
            Action = new TargetedAction("disarm", "[target name] - Fighter Ability: Attack a target. If they fail a STR check vs your STR, they drop their weapon.", "major", disarm);
        }

        private bool disarm(string targetName)
        {
            if (SentientCreature.Attack(targetName))
            {
                var entitiesOnMap = from o in SentientCreature.Location.Map.Objects where o is Entity select (Entity)o;
                var target = entitiesOnMap.Where(o => o is Entity)
                            .FirstOrDefault(e => (e as Entity).Name.ToLower() == targetName.ToLower());
                Console.WriteLine($"{SentientCreature.Name} is attempting to disarm {target.Name}!");
                var targetWeapon = target.Items.FirstOrDefault(i => i is Weapon);
                int passiveStrength = SentientCreature.AbilityScores.TotalScores["STR"];
                if (target.AbilityCheck("STR") < passiveStrength)
                {
                    if (targetWeapon != null)
                    {
                        target.DropItem(targetWeapon.Name.ToLower());
                        Console.WriteLine($"{SentientCreature.Name} disarmed {target.Name}, causing them to drop their {targetWeapon.Name}!");
                    }
                    else
                    {
                        Console.WriteLine($"{target.Name} is not holding a weapon!");
                    }
                }
                else
                {
                    Console.WriteLine($"{target.Name} kept hold of their {targetWeapon.Name}!");
                }
                return true;
            }
            return false;
        }
    }
}