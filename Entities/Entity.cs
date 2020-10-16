using System;
using System.Runtime;
using System.Collections.Generic;
using System.Linq;
using static DungeonCrawler.Dice;

namespace DungeonCrawler
{
    public abstract class Entity : IMappable, INamed
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
        public bool IsDead {get; protected set;} = false;
        public int CurrentInitiative {get; protected set;}
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
        protected virtual int _attackRangeFeet {get; set;} = 5;
        public virtual char Symbol {get; protected set;} = Symbols.Friendly;
        public List<string> Statuses {get; protected set;}
        protected List<IEntityAction> Actions = new List<IEntityAction>();
        public bool TakingTurn {get; protected set;}

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
            Actions.Add(new TargetedAction("attack", $"[target name] - Attack a target creature within {_attackRangeFeet} feet.", "major", Attack));
            Actions.Add(new NonTargetedAction("search", "- Search surroundings for items and creatures.", "major", Search));
            Actions.Add(new NonTargetedAction("hide", "- Attempt to hide from enemies", "major", Hide));
            Actions.Add(new TargetedAction("take", "[item name] - Pick up an item within 5 feet.", "minor", PickUpItem));
            Actions.Add(new TargetedAction("drop", "[item name] - Drop an item in inventory.", "minor", DropItem));
            Actions.Add(new TargetedAction("use", "[item name] - Use an item in inventory.", "minor", UseItem));
            Actions.Add(new TargetedAction("move", "[direction] [distance] - Move to an unoccupied space. Enter abbreviated direction and distance in feet. (e.g., NW 20)", "move", Move));
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
        public virtual void TakeTurn()
        {}
        // Major actions:
        public bool Search()
        {
            int rangeFeet = 20 + getModifier("WIS") * 5;
            var foundObjects = Location.GetObjectsWithinRange(rangeFeet / 5).Where(o => o is INamed && o != this).ToArray();
            if (foundObjects.Length == 0)
            {
                Console.WriteLine($"{Name} searched but couldn't find anything!");
            }
            else
            {
                Console.WriteLine($"{Name} searched and found:");
                foreach (var obj in foundObjects)
                {
                    var nObj = (INamed)obj;
                    Console.WriteLine($"- {nObj.Name} located {Location.DistanceTo(obj.Location)*5} feet {Location.GetDirectionRelativeToThis(obj.Location)}.");
                }
            }
            return true;
        }

        public bool Attack(string targetName)
        {
            var entitiesOnMap = from o in Location.Map.Objects where o is Entity select (Entity)o;
            var target = entitiesOnMap.FirstOrDefault(e => e.Name.ToLower() == targetName);
            if (target != null)
            {
                if (Location.InRangeOf(target.Location, _attackRangeFeet/5))
                {
                    Console.WriteLine($"{Name} is attacking {target.Name}...");
                    var attackResult = AttackRoll();
                    bool crit = attackResult.NaturalResults[0] == 20;
                    if (attackResult.TotalResult >= target.ArmorClass || crit)
                    {
                        Console.WriteLine(crit ? "It's a critical hit!" : "It's a hit!");
                        var damageResult = DamageRoll(crit);
                        Console.WriteLine($"{target.Name} takes {damageResult} points of damage!");
                        target.ChangeHp(damageResult*-1);
                    }
                    else
                    {
                        Console.WriteLine("It's a miss!");
                    }
                    return true;
                }
                else
                {
                    Console.WriteLine($"{target.Name} is out of range of {Name}'s attack.");
                }
            }
            else
            {
                Console.WriteLine($"{targetName} could not be found on the map.");
            }
            return false;
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
            
            var foundItem = adjacentItems.FirstOrDefault(i => i.Name.ToLower() == itemName);
            if (foundItem != null)
            {
                if (AddItem(foundItem)) 
                {
                    Location.Map.RemoveObject(foundItem);
                    Console.WriteLine($"{Name} picked up the {itemName}.");
                    return true;
                }
            }
            else
            {
                Console.WriteLine($"{Name} could not find {itemName.IndefiniteArticle()} {itemName} to pick up.");
            }
            return false;
        }
        public bool DropItem(string itemName)
        {
            var foundItem = Items.FirstOrDefault(i => i.Name.ToLower() == itemName);
            if (foundItem != null)
            {
                var adjacentObjects = Location.GetObjectsWithinRange(1).Where(o => o != this).ToArray();
                if (adjacentObjects.Length < 8)
                {
                    if (RemoveItem(foundItem))
                    {
                        var openSpaces = Location.GetAdjacentCoordinates().Where(c => 
                                         !adjacentObjects.Any(o => o.Location.X == c[0] && o.Location.Y == c[1]));
                        int[] openSpace = openSpaces.RandomElement();
                        foundItem.SetLocation(new MapPoint(openSpace[0], openSpace[1], Location.Map));
                        Location.Map.AddObject(foundItem);
                        Console.WriteLine($"{Name} dropped the {itemName}.");
                        return true;
                    }
                }
                else
                {
                    Console.WriteLine($"{Name} cannot drop the {itemName} because there are no adjacent spaces.");
                }
            }
            else
            {
                Console.WriteLine($"{Name} does not have currently have {itemName.IndefiniteArticle().ToLower()} {itemName}.");
            }
            return false;
        }
        public bool UseItem(string itemName)
        {
            return true;
        }
        // Move actions:
        public virtual bool Move(string moveInput)
        {
            try
            {
                string[] moveInputArr = moveInput.ToLower().Split(' ');
                string direction = moveInputArr[0];
                int distanceFeet = int.Parse(moveInputArr[1]);
                int distancePoints = distanceFeet / 5;
                if (distanceFeet > 0)
                {
                    if (_movementRemaining - distanceFeet >= 0)
                    {
                        var tempLocation = Location.ShallowCopy;
                        tempLocation.Translate(direction, distancePoints);
                        if (Location.Map.OnMap(tempLocation))
                        {
                            var obstruction = Location.Map.Objects.FirstOrDefault(o => MapPoint.IsPointOnLineSegment(Location, tempLocation, o.Location) && o != this);
                            if (obstruction == null)
                            {
                                Location = tempLocation;
                                _movementRemaining -= distanceFeet;
                            }
                            else
                            {
                                string output = $"{Name} cannot move {direction.ToUpper()} {distanceFeet} feet because the way is obstructed";
                                if (obstruction is INamed)
                                {
                                    var nObstruction = (INamed)obstruction;
                                    output += $" by {nObstruction.Name}";
                                }
                                Console.WriteLine(output + ".");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"{Name} cannot move outside the map.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"{Name} cannot move {distanceFeet} feet because it only has {_movementRemaining} feet of movement left.");
                    }
                }
                else
                {
                    Console.WriteLine("Distance must be greater than 0 feet.");
                }
            }
            catch (Exception)
            {
                Console.WriteLine($"Target '{moveInput}' is not valid.");
            }
            return _movementRemaining <= 0;
        }
    }
}