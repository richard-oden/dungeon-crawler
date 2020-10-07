using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonCrawler
{
    public class SentientCreature : Entity
    {
        public Race Race {get; protected set;}
        public Caste Caste {get; protected set;}
        public override int ArmorClass
        {
            get
            {
                int score = 10 + (AbilityScores.TotalScores["CON"] > AbilityScores.TotalScores["DEX"] ? getModifier("CON") : getModifier("DEX"));
                var armor = from i in Items where i is Armor select (Armor)i;
                score += armor.Sum(a => a.ArmorClassBonus);
                return score;
            }
        }
        public SentientCreature(string name, int level, char gender, int[] abilityScoreValues, Race race, Caste caste, MapPoint location = null) : base(name, level, gender, abilityScoreValues, null, location)
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

        public string GetAllStats()
        {
            string output = $"{Name}, lvl {Level.Value} {char.ToUpper(Gender)} {Race.Name} {Caste.Name}\n";
            output += $"HP: {_currentHp.Value} / {_hp} AC: {ArmorClass}\n";
            output += $"{AbilityScores.GetShortDescription()}\n";
            output += $"Inventory: {(String.IsNullOrEmpty(ListItems()) ? "Empty" : ListItems())}";
            return output;
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
                    Console.WriteLine($"{Name} already has {newArmor.Slot} armor equipped.");
                }
                if (newArmor.Material != Caste.ArmorProficiency)
                {
                    canAddItem = false;
                    Console.WriteLine($"{Name} cannot wear {newArmor.Name} because it is {newArmor.Material}. A {Caste.Name} can only wear {Caste.ArmorProficiency} armor.");
                }
            }
            if (newItem is Weapon)
            {
                var newWeapon = (Weapon)newItem;
                var heldWeapons = from i in Items where i is Weapon select (Weapon)i;
                if (heldWeapons.Any(w => w.TwoHanded) || heldWeapons.Where(w => !w.TwoHanded).Count() >= 2) 
                {
                    canAddItem = false;
                    Console.WriteLine($"{Name} cannot hold any more weapons.");
                }
                if (!Caste.WeaponProficiency.Contains(newWeapon.Type))
                {
                    canAddItem = false;
                    Console.WriteLine($"{Name} cannot use {newWeapon.Name} because {Pronouns[0].ToLower()} is a {Caste.Name}.");
                }
            }
            if (_currentWeightCarried + newItem.Weight > _maxCarryWeight) 
            {
                canAddItem = false;
                Console.WriteLine($"{newItem.Name} is too heavy for {Name} to carry.");
            }

            if (canAddItem) 
            {
                Items.Add(newItem);
                AbilityScores.AddMods(AbilityScores.ItemMods, newItem.AbilityMods);
                if (newItem is Armor)
                {
                    var newArmor = (Armor)newItem;
                }
            }

        }
    
        public override Die.Result AttackRoll()
        {
            if (!Items.Any(i => i is Weapon))
            {
                return base.AttackRoll();
            }
            else
            {
                var heldWeapons = from i in Items where i is Weapon select (Weapon)i;
                int weaponAttackBonus = heldWeapons.Sum(w => w.AttackBonus);
                return Dice.D20.RollGetResult(1, (getModifier(Caste.AbilityProficiency) + weaponAttackBonus), true);
            }
        }

        public override int DamageRoll(bool crit)
        {
            if (!Items.Any(i => i is Weapon))
            {
                return base.DamageRoll(crit);
            }
            else
            {
                var heldWeapons = from i in Items where i is Weapon select (Weapon)i;
                int weaponDamageBonus = heldWeapons.Sum(w => w.AttackBonus);
                int multiplier = crit ? 2 : 1;
                int sumDamage = 0;
                foreach (var w in heldWeapons)
                {
                    sumDamage += w.DamageDie.Roll(multiplier, (getModifier(Caste.AbilityProficiency) + weaponDamageBonus), true);
                }
                return sumDamage;
            }
        }
    }
}