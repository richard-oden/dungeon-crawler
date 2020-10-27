using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using static DungeonCrawler.Dice;
using static DungeonCrawler.ExtensionsAndHelpers;

namespace DungeonCrawler
{
    public abstract class Entity : IMappable, INamed, IDescribable
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
        protected int _experience {get; set;}
        public AbilityScores AbilityScores {get; protected set;}
        public virtual int ArmorClass => 10 + (AbilityScores.TotalScores["CON"] > AbilityScores.TotalScores["DEX"] ? getModifier("CON") : getModifier("DEX"));
        protected Die _hitDie;
        public int Hp {get; set;}
        public Stat CurrentHp {get; set;}
        public bool IsDead {get; protected set;} = false;
        public int CurrentInitiative {get; protected set;}
        public int MovementSpeedFeet => 20 + (getModifier("DEX") * 5);
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
        public virtual char Symbol => IsDead ? Symbols.Dead : Team == 0 ? Symbols.Friendly : Symbols.Hostile;
        public virtual string Description {get; protected set;}
        public List<StatusEffect> StatusEffects {get; protected set;} = new List<StatusEffect>();
        public int HiddenDc {get; protected set;} = 0;
        public int PassivePerception {get; protected set;}
        protected List<IEntityAction> Actions = new List<IEntityAction>();
        public bool TakingTurn {get; protected set;}
        public int Team {get; protected set;} // represents who this entity is allied with in combat
        
        public Entity(string name, int level, char gender, int team, int[] abilityScoreValues = null, Die hitDie = null, MapPoint location = null)
        {
            Name = name;
            Level.SetValue(level);
            Gender = gender;
            Team = team;
            AbilityScores = new AbilityScores(abilityScoreValues);

            _experience = 0;
            hitDie ??= new Die(6);
            _hitDie = hitDie;
            Hp = 0;
            for (int j = 0; j < Level.Value; j++)
            {
                Hp += _hitDie.Roll(1, getModifier("CON"));
            }
            CurrentHp = new Stat("HP", 0, Hp, Hp);

            PassivePerception = AbilityScores.TotalScores["WIS"];

            Location = location;

            Actions.Add(new TargetedAction("attack", $"[target name] - Attack a target creature within {_attackRangeFeet} feet.", "major", Attack));
            Actions.Add(new NonTargetedAction("search", "- Search surroundings for items and creatures.", "major", Search));
            Actions.Add(new NonTargetedAction("hide", "- Attempt to hide from enemies", "major", Hide));
            Actions.Add(new TargetedAction("take", "[item name] - Pick up an item within 5 feet.", "minor", TakeItem));
            Actions.Add(new TargetedAction("drop", "[item name] - Drop an item in inventory.", "minor", DropItem));
            Actions.Add(new TargetedAction("use", "[item name] - Use an item in inventory.", "minor", UseItem));
            Actions.Add(new TargetedAction("move", "[direction] [distance] - Move to an unoccupied space. Enter abbreviated direction and distance in feet. (e.g., NW 20)", "move", Move));
        }

        public virtual string GetDescription()
        {
            return $"{Name} lvl {Level.Value} has a hit die of d{_hitDie.NumSides.Value}, and total HP of {Hp}.";
        }

        public virtual string GetAllStats()
        {
            string output = $"{Name}, lvl {Level.Value} {char.ToUpper(Gender)}\n";
            output += $"HP: {CurrentHp.Value} / {Hp} AC: {ArmorClass}\n";
            output += $"{AbilityScores.GetShortDescription()}\n";
            output += $"Inventory: {ListItems()}";
            output += $"Status Effects: {ListStatusEffects()}";
            return output;
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

        public void ChangeHp(int amount)
        {
            CurrentHp.ChangeValue(amount);
            if (CurrentHp.Value == 0)
            {
                IsDead = true;
                Console.WriteLine($"{Name} has died!");
            }
        }
        
        public void SetLocation(MapPoint location)
        {   
            Location = location;
        }
        
        public virtual void TakeTurn(Combat combat)
        {
            MaintainStatusEffects();
        }

        public void RevealIfHidden(Entity enemy)
        {
            if (HiddenDc > 0)
            {
                Console.WriteLine($"{Name} was spotted and is no longer hidden!");
                HiddenDc = 0;
            }
        }

        public List<IMappable> BaseSearch()
        {
            Console.WriteLine($"{Name} is searching...");
            int perceptionRoll = D20.Roll(1, getModifier("WIS"), true);
            int perceptionCheck = perceptionRoll >= PassivePerception ? perceptionRoll : PassivePerception;
            int searchRangeFeet = (perceptionCheck - 8) * 5;

            var foundObjects = Location.GetObjectsWithinRange(searchRangeFeet / 5).Where(fO => fO is INamed && fO != this).ToList();
            foundObjects.RemoveAll(fO =>
                                  // Entity is not found if it is on another team and is successfully hiding:
                                   fO is Entity && 
                                  (fO as Entity).Team != Team && 
                                  (fO as Entity).HiddenDc > perceptionCheck ||
                                  // Object is not found if line of sight is blocked:
                                  !hasLineOfSightTo(fO));
            foreach (var obj in foundObjects)
            {
                if (obj is Entity && (obj as Entity).Team != Team) (obj as Entity).RevealIfHidden(this);
            }
            return foundObjects;
        }

        protected bool hasLineOfSightTo(IMappable target)
        {
            return !Location.Map.Objects.Any(o => MapPoint.IsPointOnLineSegment(Location, target.Location, o.Location) && o != this && o != target);
        }
        
        // =======================================================================================
        // ITEM RELATED:
        // =======================================================================================
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
            string list = Items.Select(i => i.Name).FormatToString("and");
            return String.IsNullOrEmpty(list) ? "Empty" : list;
        }
        
        // =======================================================================================
        // STATUS EFFECT RELATED:
        // =======================================================================================
        public void ApplyStatusEffect(StatusEffect newStatusEffect)
        {
            /// TODO: Figure out how to access properties with private setter
            PropertyInfo piTargetProp = this.GetType().GetProperty(newStatusEffect.TargetProp, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (piTargetProp.PropertyType == typeof(int))
            {
                piTargetProp.SetValue(this, (int)piTargetProp.GetValue(this, null) + newStatusEffect.ValueChange);
            }
            else if (piTargetProp.PropertyType == typeof(Stat))
            {
                MethodInfo miTargetProp = typeof(Stat).GetMethod("ChangeValue", BindingFlags.Public | BindingFlags.Instance);
                miTargetProp.Invoke(piTargetProp.GetValue(this), new object[] {newStatusEffect.ValueChange});
            }
            else
            {
                throw new InvalidEntityPropertyException($"{newStatusEffect.TargetProp} is not a valid property for {Name}.");
            }
            Console.WriteLine($"{Name} is {newStatusEffect.Name}. {Pronouns[2]} {newStatusEffect.TargetProp.FromTitleOrCamelCase()} is {(newStatusEffect.ValueChange >= 0 ? "increased" : "decreased")} by {Math.Abs(newStatusEffect.ValueChange)}.");
        }

        public void UnapplyStatusEffect(StatusEffect currentStatusEffect)
        {
            var reversedStatusEffect = new StatusEffect
            (
                "recovering from " + currentStatusEffect.Name, 
                (currentStatusEffect.HasCoolDown ? currentStatusEffect.Duration : 0), 
                currentStatusEffect.TargetProp, 
                currentStatusEffect.ValueChange*-1, 
                currentStatusEffect.Recurring,
                hasCoolDown: currentStatusEffect.HasCoolDown
            );
            ApplyStatusEffect(reversedStatusEffect);
        }

        public void MaintainStatusEffects()
        {
            foreach (var statusEffect in StatusEffects)
            {
                statusEffect.DecrementDuration();
                if (statusEffect.Recurring) ApplyStatusEffect(statusEffect);
                if (statusEffect.Duration == 0) 
                {
                    if (statusEffect.UndoWhenFinished) UnapplyStatusEffect(statusEffect);
                    Console.WriteLine($"{Name} is no longer {statusEffect.Name}.");
                }
            }
            StatusEffects.RemoveAll(se => se.Duration == 0);
        }
        
        public string ListStatusEffects()
        {
            string list = StatusEffects.Select(sE => sE.Name).FormatToString("and");
            return String.IsNullOrEmpty(list) ? "None" : list;
        }
        
        // =======================================================================================
        // ACTIONS:
        // =======================================================================================
        
        // Major actions:   
        public virtual bool Search()
        {
            BaseSearch();
            return true;
        }
        
        public virtual bool Attack(string targetName)
        {
            var entitiesOnMap = from o in Location.Map.Objects where o is Entity select (Entity)o;
            var target = entitiesOnMap.FirstOrDefault(e => e.Name.ToLower() == targetName.ToLower());
            if (target != null)
            {
                if (Location.InRangeOf(target.Location, _attackRangeFeet/5))
                {
                    if (hasLineOfSightTo(target))
                    {
                        if (target.HiddenDc <= PassivePerception)
                        {
                            target.RevealIfHidden(this);
                            Console.WriteLine($"{Name} is attacking {target.Name}...");
                            var attackResult = AttackRoll();
                            bool crit = attackResult.NaturalResults[0] == 20;
                            if (attackResult.TotalResult >= target.ArmorClass || crit)
                            {
                                Console.WriteLine(crit ? "It's a critical hit!" : "It's a hit!");
                                var damageResult = DamageRoll(crit);
                                Console.WriteLine($"{target.Name} takes {damageResult} points of damage!");
                                target.ChangeHp(damageResult*-1);
                                if (target is INpc) (target as INpc).DamagedBy(this);
                            }
                            else
                            {
                                Console.WriteLine("It's a miss!");
                                if (target is INpc) (target as INpc).AttackedBy(this);
                            }
                            return true;
                        }
                        else
                        {
                            Console.WriteLine($"{Name} cannot find {target.Name} because they are hidden!");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"{Name} cannot hit {target.Name} because {Pronouns[2].ToLower()} line of sight is blocked!");
                    }
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
        
        public virtual bool Hide()
        {
            Console.WriteLine($"{Name} is attempting to hide..");
            HiddenDc = D20.Roll(1, getModifier("DEX"), true);
            return true;
        }
        
        // Minor actions:
        public virtual bool TakeItem(string itemName)
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
                Console.WriteLine($"{Name} could not find {itemName.IndefiniteArticle().ToLower()} {itemName} to pick up.");
            }
            return false;
        }
        
        public virtual bool DropItem(string itemName)
        {
            var foundItem = Items.FirstOrDefault(i => i.Name.ToLower() == itemName);
            if (foundItem != null)
            {
                var adjacentObjects = Location.GetObjectsWithinRange(1).Where(o => o != this).ToArray();
                if (adjacentObjects.Length < 8)
                {
                    if (RemoveItem(foundItem))
                    {
                        int[] openSpace = Location.Map.GetOpenSpaces(Location.GetAdjacentCoordinates()).RandomElement();
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
        
        public virtual bool UseItem(string itemName)
        {
            var foundItem = Items.FirstOrDefault(i => i.Name.ToLower() == itemName.ToLower());
            if (foundItem != null)
            {
                if (foundItem is Consumable)
                {
                    var foundConsumable = (Consumable)foundItem;
                    Console.WriteLine($"{Name} used the {foundConsumable.Name}.");
                    foreach (var statusEffect in foundConsumable.StatusEffects)
                    {
                        StatusEffects.Add(statusEffect);
                        ApplyStatusEffect(statusEffect);
                    }
                    Items.Remove(foundItem);
                    return true;
                }
                else
                {
                    Console.WriteLine($"{foundItem.Name} is not consumable.");
                }
            }
            else
            {
                Console.WriteLine($"{Name} does not have currently have {itemName.IndefiniteArticle().ToLower()} {itemName}.");
            }
            return false;
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
                                Console.WriteLine($"{Name} moved {direction.ToUpper()} {distanceFeet} feet.");
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