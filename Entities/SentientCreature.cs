using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonCrawler
{
    public abstract class SentientCreature : Entity
    {
        public Race Race {get; protected set;}
        public Caste Caste {get; protected set;}
        public int RaceActionUses {get; protected set;} = 2;
        public int CasteActionUses {get; protected set;} = 3;
        public override string Description
        {
            get
            {
                var description = $"{Name} is {Race.Name.IndefiniteArticle().ToLower()} {Race.Name} {Caste.Name}.";

                string hpString;
                if (CurrentHp.Value == Hp) hpString = "uninjured";
                else if (CurrentHp.Value > Hp * .75) hpString = "mildly injured";
                else if (CurrentHp.Value > Hp * .5) hpString = "fairly injured";
                else if (CurrentHp.Value > Hp * .25) hpString = "greatly injured";
                else if (CurrentHp.Value > 0) hpString = "near death";
                else hpString = "dead";
                description += $" {Pronouns[0]} appears {hpString}.";

                var weaponsList = Items.Where(i => i is Weapon).ToList(); 
                if (weaponsList.Count > 0)
                {
                    var weaponsString = weaponsList.Select(w => 
                        $"{w.Name.IndefiniteArticle().ToLower()} {w.Name}").FormatToString("and").ToLower();
                    description += $" {Pronouns[0]} is wielding {weaponsString}.";
                }
                var armorList = Items.Where(i => i is Armor).ToList(); 
                if (weaponsList.Count > 0)
                {
                    var armorString = armorList.Select(w => 
                        $"{w.Name.IndefiniteArticle().ToLower()} {w.Name}").FormatToString("and").ToLower();
                    description += $" {Pronouns[0]} is wearing {armorString}.";
                }

                return description;
            }
        }
        public override int ArmorClass
        {
            get
            {
                int score = 10 + (AbilityScores.TotalScores["CON"] > AbilityScores.TotalScores["DEX"] ? GetModifier("CON") : GetModifier("DEX"));
                var armor = from i in Items where i is Armor select (Armor)i;
                score += armor.Sum(a => a.ArmorClassBonus);
                return score;
            }
        }
        protected override int _attackRangeFeet
        {
            get
            {
                var equippedWeapon = (Weapon)Items.FirstOrDefault(i => i is Weapon);
                if (equippedWeapon != null)
                {
                    return equippedWeapon.Range;
                }
                else
                {
                    return base._attackRangeFeet;
                }
            }
        }
        protected override string _damageType
        {
            get
            {
                var heldWeapon = Items.FirstOrDefault(i => i is Weapon);
                if (heldWeapon != null)
                {
                    return (heldWeapon as Weapon).DamageType;
                }
                else
                {
                    return base._damageType;
                }
            }
        }
        public SentientCreature(string name, int level, char gender, Race race, Caste caste, int team, 
                int[] abilityScoreValues = null, MapPoint location = null, List<Item> items = null) : 
            base(name, level, gender, team, abilityScoreValues, null, location, items)
        {
            Race = race;
            Race.SentientCreature = this;
            Caste = caste;
            Caste.SentientCreature = this;
            _hitDie = race.HitDie;
            AbilityScores.AddMods(AbilityScores.RacialMods, Race.AbilityMods);
        }

        public override string GetAllStats()
        {
            string output = $"{Name}, lvl {Level.Value} {char.ToUpper(Gender)} {Race.Name} {Caste.Name}\n";
            output += $"HP: {GetHpDescription()} AC: {ArmorClass}\n";
            output += $"{AbilityScores.GetShortDescription()}\n";
            output += $"Inventory: {ListItems()} ({_currentWeightCarried}/{_maxCarryWeight} lbs)\n";
            output += $"Status Effects: {ListStatusEffects()}\n";
            if (HiddenDc > 0) output += $"Currently hidden with DC of {HiddenDc}.\n";
            return output;
        }

        public override bool AddItem(Item newItem)
        {
            bool canAddItem = true;
            if (newItem is Armor)
            {
                var newArmor = (Armor)newItem;
                var equippedArmor = from i in Items where i is Armor select (Armor)i;
                if (equippedArmor.Any(a => a.Slot.ToLower() == newArmor.Slot.ToLower())) 
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
                if (heldWeapons.Any(w => w.TwoHanded) || 
                    heldWeapons.Count() >= 1 && newWeapon.TwoHanded ||
                    heldWeapons.Count() >= 2) 
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
            }

            return canAddItem;
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
                return Dice.D20.RollGetResult(1, (GetModifier(Caste.AbilityProficiency) + weaponAttackBonus), true);
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
                    sumDamage += w.DamageDie.Roll(multiplier, (GetModifier(Caste.AbilityProficiency) + weaponDamageBonus), true);
                }
                return sumDamage;
            }
        }
    }
}