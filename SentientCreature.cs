using System;
using System.Collections.Generic;
using System.Linq;
using static DungeonCrawler.Dice;

namespace DungeonCrawler
{
    public abstract class SentientCreature : Entity
    {
        public Race Race {get; protected set;}
        public Caste Caste {get; protected set;}
        public SentientCreature(string name, int level, char gender, int[] abilityScoreValues, Race race, Caste caste) : base(name, level, gender, abilityScoreValues, null)
        {
            Race = race;
            Caste = caste;
            _hitDie = race.HitDie;
            AbilityScores.AddMods(AbilityScores.RacialMods, Race.AbilityMods);
        }

        public override string GetDescription()
        {
            return $"{Name} is a level {Level.Value} {Race.Name} {Caste.Name}. {Pronouns[2]} hit die is a d{_hitDie.NumSides.Value}, and {Pronouns[2].ToLower()} total HP is {_hp}.";
        }

        public override void AddItem(Item newItem)
        {
            bool canAddItem = true;
            if (newItem is Armor)
            {
                var newArmor = (Armor)newItem;
                var equippedArmor = from i in Items where i is Armor select (Armor)i;
                if (equippedArmor.Any(a => a.Slot == newArmor.Slot)) 
                {
                    canAddItem = false;
                    Console.WriteLine($"{this.Name} already has {newArmor.Slot} armor equipped!");
                }
            }
            if (newItem is Weapon)
            {
                var newWeapon = (Weapon)newItem;
                var heldWeapons = from i in Items where i is Weapon select (Weapon)i;
                if (heldWeapons.Any(w => w.TwoHanded) || heldWeapons.Where(w => !w.TwoHanded).Count() >= 2) 
                {
                    canAddItem = false;
                    Console.WriteLine($"{this.Name} cannot hold any more weapons!");
                }
            }
            if (_currentWeightCarried + newItem.Weight > _maxCarryWeight) 
            {
                canAddItem = false;
                Console.WriteLine($"{newItem.Name} is too heavy for {this.Name} to carry!");
            }

            if (canAddItem) 
            {
                Items.Add(newItem);
                AbilityScores.AddMods(AbilityScores.ItemMods, newItem.AbilityMods);
            }

        }
    }
}