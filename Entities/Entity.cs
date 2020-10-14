using System;
using System.Runtime;
using System.Collections.Generic;
using System.Linq;
using static DungeonCrawler.Dice;

namespace DungeonCrawler
{
    public abstract class Entity : IMappable
    {
        public string Name {get; protected set;}
        public char Gender {get; protected set;}
        public string[] Pronouns
        {
            get
            {
                if (Gender == 'm') return new[] {"He", "Him", "His"};
                else if (Gender == 'f') return new[] {"She", "Her", "Her"};
                else if (Gender == 'n') return new[] {"They", "Them", "Their"};
                else return new [] {"It", "It", "Its"};
            }
        }
        public Stat Level {get; protected set;} = new Stat("Level", 1, 100);
        protected int _experience;
        public AbilityScores AbilityScores {get; protected set;}
        public virtual int ArmorClass => 10 + (AbilityScores.TotalScores["CON"] > AbilityScores.TotalScores["DEX"] ? getModifier("CON") : getModifier("DEX"));
        protected Die _hitDie;
        protected int _hp;
        protected Stat _currentHp;
        public int CurrentInitiative {get; protected set;}
        public bool IsDead {get; protected set;} = false;
        public int MovementSpeedFeet => 30 + (getModifier("DEX") * 5);
        protected int _movementRemaining;
        protected double _maxCarryWeight => AbilityScores.TotalScores["STR"] * 15;
        protected double _currentWeightCarried
        {
            get
            {
                double totalWeight = 0;
                foreach (var i in Items) totalWeight += i.Weight;
                return totalWeight;
            }
        }
        public List<Item> Items {get; protected set;} = new List<Item>();
        public MapPoint Location {get; protected set;}
        public virtual char Symbol {get; protected set;} = Symbols.Friendly;
        protected List<IEntityAction> Actions = new List<IEntityAction>();

        public Entity(string name, int level, char gender, int[] abilityScoreValues = null, Die hitDie = null, MapPoint location = null)
        {
            Name = name;
            Level.SetValue(level);
            Gender = gender;
            AbilityScores = new AbilityScores(abilityScoreValues);

            _experience = 0;
            hitDie ??= new Die(6);
            _hitDie = hitDie;
            _hp = 0;
            for (int j = 0; j < Level.Value; j++)
            {
                _hp += _hitDie.Roll(1, getModifier("CON"));
            }
            _currentHp = new Stat("HP", 0, _hp, _hp);

            Location = location;

            // Actions.Add(new TargetedAction("basic attack", "[target name] - Direct a basic attack at a creature in range", "major", Attack));
            Actions.Add(new NonTargetedAction("search", "- Search surroundings for items and creatures", "major", Search));
            Actions.Add(new NonTargetedAction("hide", "- Attempt to hide from enemies", "major", Hide));
            Actions.Add(new TargetedAction("take", "[item name] - Pick up an item within 5 feet.", "minor", PickUpItem));
            Actions.Add(new TargetedAction("drop", "[item name] - Drop an item in inventory.", "minor", DropItem));
            Actions.Add(new TargetedAction("use", "[item name] - Use an item in inventory.", "minor", UseItem));
        }

        protected int getModifier(string abilityScore)
        {
            return (int)Math.Floor(((double)AbilityScores.TotalScores[abilityScore] - 10.0) / 2.0);
        }

        public int AbilityCheck(string abil)
        {
            if (AbilityScores.BaseScores.ContainsKey(abil))
            {
                Console.WriteLine($"Rolling {AbilityScores.BaseScores[abil].Name} check for {this.Name}...");
                return D20.Roll(1, getModifier(abil), true);
            }
            else 
            {
                throw new InvalidAbilityException($"Ability '{abil}' in entity '{this.Name}' is not valid!");
            }
        }

        public virtual string GetDescription()
        {
            return $"{Name} lvl {Level.Value} has a hit die of d{_hitDie.NumSides.Value}, and total HP of {_hp}.";
        }

        public void ChangeHp(int amount)
        {
            _currentHp.ChangeValue(amount);
            if (_currentHp.Value == 0)
            {
                IsDead = true;
                Console.WriteLine($"{Name} has died!");
            }
        }
        
        public virtual bool AddItem(Item newItem)
        {
            if (_currentWeightCarried + newItem.Weight <= _maxCarryWeight)
            {
                Items.Add(newItem);
                return true;
            }
            else
            {
                Console.WriteLine($"{Name} cannot carry any more weight!");
                return false;
            }
        }
        
        public bool RemoveItem(Item heldItem)
        {
            if (Items.Contains(heldItem))
            {
                Items.Remove(heldItem);
                return true;
            }
            else
            {
                Console.WriteLine($"{Name} does not currently have {heldItem.Name.IndefiniteArticle()} {heldItem.Name}.");
                return false;
            }
        }
        
        public string ListItems()
        {
            var itemNames = Items.Select(i => i.Name);
            return itemNames.FormatToString("and");
        }
        
        public void InitiativeRoll(int mod = 0)
        {
            CurrentInitiative = D20.Roll(1, (getModifier("DEX") + mod));
        }
        
        public virtual Die.Result AttackRoll()
        {
            return Dice.D20.RollGetResult(1, getModifier("DEX"), true);
        }

        public virtual int DamageRoll(bool crit)
        {
            int multiplier = crit ? 2 : 1;
            return Dice.D4.Roll(multiplier, getModifier("STR"), true);
        }

        public void SetLocation(MapPoint location)
        {   
            Location = location;
        }

        // Returns true if successful move:
        public bool Move(string direction, int distanceFeet)
        {
            // Enforce bounds checking:
            var movementRange = new Stat($"{Name} Movement Range", 0, MovementSpeedFeet, distanceFeet);
            int distancePoints = movementRange.Value / 5;
            var tempLocation = Location.ShallowCopy;
            tempLocation.Translate(direction, distancePoints);
            if (Location.Map.Objects.Any(o => o.Location.X == tempLocation.X && o.Location.Y == tempLocation.Y))
            {
                Console.WriteLine($"Cannot move to ({tempLocation.X}, {tempLocation.Y}) because it is blocked!");
                Console.ReadLine();
                return false;
            }
            else
            {
                Location = tempLocation;
                return true;
            }
        }
        public virtual void TakeTurn()
        {}
        // Major actions:
        // public void DoAttack(string targetName)
        // {
        //     var target = (Entity)targetEntity;
        //     Die.Result attackRollResult = AttackRoll();
        //     bool crit = attackRollResult.NaturalResults[0] == 20;
        //     if (crit) 
        //     {
        //         Console.WriteLine($"{Name} landed a critical hit on {target.Name}!");
        //     }
        //     else if (attackRollResult.TotalResult >= target.ArmorClass)
        //     {
        //         int damage = DamageRoll(crit);
        //         Console.WriteLine($"{Name} inflicted {damage} points of damage on {target.Name}!");
        //         target.ChangeHp(-1*damage);
        //     }
        //     else
        //     {
        //         Console.WriteLine($"{Name} missed {target.Name}!");
        //     }
        // }
        public bool Search()
        {
            return true;
        }
        public bool Hide()
        {
            return true;
        }
        // Minor actions:
        public bool PickUpItem(string itemName)
        {
            var adjacentObjects = Location.GetObjectsWithinRange(1);
            var adjacentItems = from o in adjacentObjects where o is Item select (Item)o;
            
            var foundItem = adjacentItems.FirstOrDefault(i => i.Name == itemName);
            if (foundItem != null)
            {
                if (AddItem(foundItem)) Location.Map.RemoveObject(foundItem);
                return true;
            }
            else
            {
                Console.WriteLine($"Item {itemName} was not found!");
                return false;
            }
        }
        public bool DropItem(string itemName)
        {
            return true;
        }
        public bool UseItem(string itemName)
        {
            return true;
        }
        // Move actions:
        public virtual void DoMove(string input)
        {
            string[] inputArr = input.ToLower().Split(' ');;
            Move(inputArr[0], int.Parse(inputArr[1]));
        }
    }
}