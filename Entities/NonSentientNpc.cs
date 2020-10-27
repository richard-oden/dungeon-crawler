using System;
using System.Collections.Generic;
using System.Linq;
using static DungeonCrawler.Dice;
using static DungeonCrawler.ExtensionsAndHelpers;

namespace DungeonCrawler
{
    public class NonSentientNpc : Entity, INpc
    {
        public Aggression Aggression {get; protected set;}
        public List<Entity> KnownEntities {get; protected set;} = new List<Entity>();
        public List<Item> KnownItems {get; protected set;} = new List<Item>();
        public List<Entity> Attackers {get; protected set;} = new List<Entity>();
        public NonSentientNpc(string name, int level, char gender, int team, Aggression aggression, int[] abilityScoreValues = null, Die hitDie = null, MapPoint location = null) : 
            base(name, level, gender, team, abilityScoreValues, hitDie, location)
        {
            Aggression = aggression;
        }

        private void addToKnown(List<IMappable> knownObjects)
        {
            foreach (var obj in knownObjects)
            {
                if (obj is Entity) KnownEntities.Add(obj as Entity);
                else if (obj is Item) KnownItems.Add(obj as Item);
            }
        }
        
        public override bool Search()
        {
            var knownObjects = BaseSearch();
            // Nonsentient npcs don't care about weapons or armor
            knownObjects.RemoveAll(o => o is Armor || o is Weapon); //||
                                  // And are only interested in enemies
                                  //(o is Entity && (o as Entity).Team == this.Team));
            addToKnown(knownObjects);
            return true;
        }

        public void AttackedBy(Entity enemy)
        {
            if (!Attackers.Contains(enemy)) Attackers.Add(enemy);
            if (KnownEntities.Contains(enemy)) KnownEntities.MoveElement(enemy, -1);
        }

        public void DamagedBy(Entity enemy)
        {
            if (!Attackers.Contains(enemy)) Attackers.Add(enemy);
            if (KnownEntities.Contains(enemy)) KnownEntities.MoveElement(enemy, -2);
        }

        public void Prioritize()
        {
            if (KnownItems.Count > 0)
            {
                KnownItems.OrderBy(i => i.Location.DistanceTo(this.Location)) // order items by closest
                        .ThenByDescending(i => i is Consumable); // give priority to consumables
            }
            if (KnownEntities.Count > 0)
            {
                KnownEntities.OrderBy(e => e.Location.DistanceTo(this.Location)) // order enemies by closest
                            .ThenBy(e => e.CurrentHp.Value) // and those that have less hp remaining
                            .ThenByDescending(e => hasLineOfSightTo(e)); // and are in line of sight
            }
        }

        public void PathfindTo(MapPoint finalDestination)
        {
            Console.Clear();
            bool moveDone = false;
            while (_movementRemaining > 0 && !moveDone)
            {
                IMappable obstruction = Location.Map.Objects.FirstOrDefault(o => 
                                        MapPoint.IsPointOnLineSegment(this.Location, finalDestination, o.Location) &&
                                        o != this);
                var destination = obstruction != null ? obstruction.Location : finalDestination;

                string direction = Location.DirectionTo(destination);
                int distance = Location.DistanceTo(destination);
                
                // if distance is greater than movement remaining, only move up to remaining movement:
                Move($"{direction} {(distance <= _movementRemaining ? distance : _movementRemaining)}");

                // attempt to move around obstruction if present:
                if (obstruction != null && _movementRemaining >= 5)
                {
                    var openSpace = Location.Map.GetOpenSpaces(Location.GetAdjacentCoordinates())
                                                .RandomElement().ToMapPoint(Location.Map);
                    var tempLocation = Location.ShallowCopy;
                    Move($"{Location.DirectionTo(openSpace)} {5}");
                }
                else
                {
                    moveDone = true;
                }
            }
            Location.Map.PrintMap();
        }
        
        public void PathfindTo(IMappable target)
        {
            bool moveDone = false;
            while (_movementRemaining > 0 && !moveDone)
            {
                // Console.WriteLine("pathfinding");
                IMappable obstruction = Location.Map.Objects.FirstOrDefault(o => 
                                        MapPoint.IsPointOnLineSegment(this.Location, target.Location, o.Location) &&
                                        o != this && o != target);
                MapPoint destination = (obstruction == null ? target : obstruction).Location;

                string direction = Location.DirectionTo(destination);

                // move up to 5 feet if item, up to attack range if entity:
                int distance = (Location.DistanceTo(destination) * 5) - (target is Entity ? _attackRangeFeet : 5);
                
                // if distance is greater than movement remaining, only move up to remaining movement:
                Move($"{direction} {(distance < _movementRemaining ? distance : _movementRemaining)}");
                Location.Map.PrintMap();
                PressAnyKeyToContinue();
                Console.Clear();

                // attempt to move around obstruction if present:
                if (obstruction != null && _movementRemaining >= 5)
                {
                    Console.WriteLine("obstruction encounted");
                    var openSpace = Location.Map.GetOpenSpaces(Location.GetAdjacentCoordinates())
                                                .RandomElement().ToMapPoint(Location.Map);
                    var tempLocation = Location.ShallowCopy;
                    Move($"{Location.DirectionTo(openSpace)} {5}");
                    Location.Map.PrintMap();
                    PressAnyKeyToContinue();
                    Console.Clear();
                }
                else
                {
                    moveDone = true;
                }
            }
        }

        public void Flee()
        {
            Console.WriteLine($"{Name} is attempting to flee!");
            PressAnyKeyToContinue();
            var allOpenSpaces = Location.Map.GetAllOpenSpaces();
            if (allOpenSpaces.Length > 0)
            {
                var allOpenMapPoints = new List<MapPoint>();
                foreach (var openSpace in allOpenSpaces) allOpenMapPoints.Add(openSpace.ToMapPoint(Location.Map));

                // order all open spaces by the sum of their distance to every enemy:
                allOpenMapPoints.OrderByDescending(mP => KnownEntities.Sum(e => e.Location.DistanceTo(mP)));
                
                // the first one should be the farthest from all enemies:
                PathfindTo(allOpenMapPoints[0]);
            }
            else
            {
                Console.WriteLine($"There is nowhere for {Name} to run!");
                PressAnyKeyToContinue();
            }
        }

        public void Engage(IMappable target)
        {
            int range = target is Entity ? _attackRangeFeet/5 : 1;
            while (_movementRemaining > 0)
            {
                if (Location.InRangeOf(target.Location, range))
                {
                    if (target is Entity)
                    {
                        Attack((target as Entity).Name);
                    }
                    else if (target is Item)
                    {
                        TakeItem((target as Item).Name);
                    }
                    break;
                }
                else
                {
                    PathfindTo(target);
                }
            }
        }

        public override void TakeTurn(Combat combat)
        {
            var actionsRemaining = new List<string>() {"major", "minor"};
            _movementRemaining = MovementSpeedFeet;
            MaintainStatusEffects();

            //while (actionsRemaining.Count > 0)
            //{
                Console.Clear();
                Prioritize();
                // Console.WriteLine("prioritized");

                // if hp is over half:
                if (CurrentHp.Value > (Hp / 2))
                {
                    // Console.WriteLine("hp greater than half");
                    // if an entity is known, attempt to attack:
                    if (KnownEntities.Count > 0 && actionsRemaining.Contains("major"))
                    {
                        Engage(KnownEntities[0]);
                        actionsRemaining.Remove("major");
                    }
                    // if npc has been attacked by unknown entity, search:
                    if (Attackers.Count > KnownEntities.Count  && actionsRemaining.Contains("minor"))
                    {
                        Search();
                        actionsRemaining.Remove("minor");
                    }
                }
                // if hp is less than or equal to half and above 1/4th:
                else if (CurrentHp.Value > (Hp / 4))
                {
                    // if holding a consumable, use item:
                    var heldConsumable = (Consumable)(Items.FirstOrDefault(i => i is Consumable));
                    if (heldConsumable != null && actionsRemaining.Contains("major"))
                    {
                        UseItem(heldConsumable.Name);
                        actionsRemaining.Remove("major");
                    }
                    // if an item is known, attempt to pick it up
                    if (KnownItems.Count > 0 && 
                        (_maxCarryWeight - _currentWeightCarried) >= KnownItems[0].Weight &&
                        actionsRemaining.Contains("minor"))
                    {
                        Engage(KnownItems[0]);
                        actionsRemaining.Remove("minor");
                    }
                    // if npc has been attacked by unknown entity, search:
                    if (Attackers.Count > KnownEntities.Count && actionsRemaining.Contains("minor"))
                    {
                        Search();
                        actionsRemaining.Remove("minor");
                    }
                    // if an entity is known, attempt to attack:
                    if (KnownEntities.Count > 0 && actionsRemaining.Contains("major"))
                    {
                        Engage(KnownEntities[0]);
                        actionsRemaining.Remove("major");
                    }
                    // if not hidden, attempt to hide:
                    if (this.HiddenDc <= 0 && actionsRemaining.Contains("minor"))
                    {
                        Hide();
                    }
                }
                // if hp is below 1/4th:
                else
                {
                    Flee();
                    // if not hidden, attempt to hide:
                    if (this.HiddenDc <= 0 && actionsRemaining.Contains("minor"))
                    {
                        Hide();
                    }
                    // if holding a consumable, use item:
                    var heldConsumable = (Consumable)(Items.FirstOrDefault(i => i is Consumable));
                    if (heldConsumable != null && actionsRemaining.Contains("major"))
                    {
                        UseItem(heldConsumable.Name);
                        actionsRemaining.Remove("major");
                    }
                    // if an item is known, attempt to pick it up
                    if (KnownItems.Count > 0 && 
                        (_maxCarryWeight - _currentWeightCarried) >= KnownItems[0].Weight &&
                        actionsRemaining.Contains("minor"))
                    {
                        Engage(KnownItems[0]);
                        actionsRemaining.Remove("minor");
                    }
                }
            //}
            Console.WriteLine($"{Name}'s turn is over.");
            PressAnyKeyToContinue();
        }
    }
}