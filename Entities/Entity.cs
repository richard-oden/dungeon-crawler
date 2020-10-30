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
        public virtual int ArmorClass => 10 + (AbilityScores.TotalScores["CON"] > AbilityScores.TotalScores["DEX"] ? GetModifier("CON") : GetModifier("DEX"));
        protected Die _hitDie;
        public int Hp {get; set;} = 0;
        public Stat CurrentHp {get; set;}
        public int TempHp {get; set;}
        public bool IsDead {get; protected set;} = false;
        public int CurrentInitiative {get; protected set;}
        protected int _baseMovementSpeed {get; set;}
        public int MovementSpeedFeet => _baseMovementSpeed + (GetModifier("DEX") * 5);
        protected int _movementRemaining {get; set;}
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
        public bool TakingTurn {get; protected set;}
        public int Team {get; protected set;} // represents who this entity is allied with in combat
        
        public Entity(string name, int level, char gender, int team, int[] abilityScoreValues = null, Die hitDie = null, MapPoint location = null)
        {
            Name = name;
            Level.SetValue(level);
            Gender = gender;
            Team = team;
            AbilityScores = new AbilityScores(abilityScoreValues);

            _baseMovementSpeed = 20;

            _experience = 0;
            hitDie ??= new Die(6);
            _hitDie = hitDie;
            for (int j = 0; j < Level.Value; j++)
            {
                Hp += _hitDie.Roll(1, GetModifier("CON"));
            }
            CurrentHp = new Stat("HP", 0, Hp, Hp);

            PassivePerception = AbilityScores.TotalScores["WIS"];

            Location = location;
        }

        public virtual string GetDescription()
        {
            return $"{Name} lvl {Level.Value} has a hit die of d{_hitDie.NumSides.Value}, and total HP of {Hp}.";
        }

        public string GetHpDescription()
        {
            string maxHp = TempHp > 0 ? $"{Hp + TempHp} ({Hp} + {TempHp} temp)" : Hp.ToString();
            string currentHp = TempHp > 0 ? $"{CurrentHp.Value + TempHp}" : $"{CurrentHp.Value.ToString()}";

            return $"{currentHp} / {maxHp}";
        }
        
        public virtual string GetAllStats()
        {
            string output = $"{Name}, lvl {Level.Value} {char.ToUpper(Gender)}\n";
            output += $"HP: {GetHpDescription()} AC: {ArmorClass}\n";
            output += $"{AbilityScores.GetShortDescription()}\n";
            output += $"Inventory: {ListItems()}";
            output += $"Status Effects: {ListStatusEffects()}";
            return output;
        }

        public int GetModifier(string abilityScore)
        {
            return (int)Math.Floor(((double)AbilityScores.TotalScores[abilityScore] - 10.0) / 2.0);
        }

        public int AbilityCheck(string abil, bool printOutput = true)
        {
            if (AbilityScores.BaseScores.ContainsKey(abil))
            {
                if (printOutput)
                {
                    Console.WriteLine($"{AbilityScores.BaseScores[abil].Name} check for {Name}:");
                }
                return D20.Roll(1, GetModifier(abil), printOutput);
            }
            else 
            {
                throw new InvalidAbilityException($"Ability '{abil}' in entity '{Name}' is not valid!");
            }
        }
        
        public void InitiativeRoll(int mod = 0)
        {
            CurrentInitiative = AbilityCheck("DEX", false);
        }
        
        public virtual Die.Result AttackRoll()
        {
            return Dice.D20.RollGetResult(1, GetModifier("DEX"), true);
        }

        public virtual int DamageRoll(bool crit)
        {
            int multiplier = crit ? 2 : 1;
            return Dice.D4.Roll(multiplier, GetModifier("STR"), true);
        }

        public void ChangeHp(int amount)
        {
            if (amount < 0) // if less than 0, hp is being decreased
            {
                if (TempHp > 0) // if entity has TempHp, reduce TempHp before CurrentHp
                {
                    TempHp += amount;
                    if (TempHp < 0) // if TempHp has been depleted, change CurrentHp
                    {
                        amount = TempHp; // amount now equals left over damage not absorbed by TempHp
                        TempHp = 0;
                    }
                    else // if all damage was absorbed by TempHp, remaining damage is 0
                    {
                        amount = 0; 
                    }
                }
                else if (TempHp < 0) // reset TempHp back to 0 incase it is less than 0
                {
                    TempHp = 0;
                }
            }
            CurrentHp.ChangeValue(amount);
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
            int perceptionRoll = AbilityCheck("WIS");
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
        public void AddStatusEffect(StatusEffect statusEffect)
        {
            StatusEffects.Add(statusEffect);
            ApplyStatusEffect(statusEffect);
        }
        
        public void ApplyStatusEffect(StatusEffect statusEffect)
        {
            PropertyInfo piTargetProp = this.GetType().GetProperty(statusEffect.TargetProp, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (piTargetProp.PropertyType == typeof(int))
            {
                piTargetProp.SetValue(this, (int)piTargetProp.GetValue(this, null) + statusEffect.ValueChange);
            }
            else if (piTargetProp.PropertyType == typeof(Stat))
            {
                MethodInfo miTargetProp = typeof(Stat).GetMethod("ChangeValue", BindingFlags.Public | BindingFlags.Instance);
                miTargetProp.Invoke(piTargetProp.GetValue(this), new object[] {statusEffect.ValueChange});
            }
            else if (piTargetProp.PropertyType == typeof(AbilityScores))
            {
                MethodInfo miTargetProp = typeof(AbilityScores).GetMethod("AddMods", BindingFlags.Public | BindingFlags.Instance);
                var abilityScoreChange = new Dictionary<string, int>() {{statusEffect.TargetedAbilityScore, statusEffect.ValueChange}};
                miTargetProp.Invoke(piTargetProp.GetValue(this), new object[] {AbilityScores.TempMods, abilityScoreChange});
            }
            else
            {
                throw new InvalidEntityPropertyException($"{statusEffect.TargetProp} is not a valid property for {Name}.");
            }
            string targetPropText = statusEffect.TargetedAbilityScore != null ? statusEffect.TargetedAbilityScore : statusEffect.TargetProp.FromTitleOrCamelCase();
            string increasedOrDecreased = statusEffect.ValueChange >= 0 ? "increased" : "decreased";
            Console.WriteLine($"{Name} is {statusEffect.Name}. {Pronouns[2]} {targetPropText} is {increasedOrDecreased} by {Math.Abs(statusEffect.ValueChange)}.");
        }

        public void UnapplyStatusEffect(StatusEffect currentStatusEffect)
        {
            var reversedStatusEffect = new StatusEffect
            (
                "recovering from " + currentStatusEffect.Name, 
                (currentStatusEffect.HasCoolDown ? currentStatusEffect.Duration : 0), 
                currentStatusEffect.TargetProp, 
                currentStatusEffect.ValueChange*-1,
                currentStatusEffect.TargetedAbilityScore,
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
            string list = StatusEffects.Select(sE => $"{sE.Name} ({sE.Duration} turns left)").FormatToString("and");
            return String.IsNullOrEmpty(list) ? "None" : list;
        }
        
        // =======================================================================================
        // ACTIONS:
        // (each action returns true if it was successful)
        // =======================================================================================
         
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
                            }
                            else
                            {
                                Console.WriteLine("It's a miss!");
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
            HiddenDc = AbilityCheck("DEX");
            return true;
        }
        
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