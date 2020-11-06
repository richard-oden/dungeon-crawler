using System;
using System.Collections.Generic;
using System.Linq;
using static DungeonCrawler.ExtensionsAndHelpers;

namespace DungeonCrawler
{
    public class Player : SentientCreature
    {
        public override char Symbol => IsDead ? Symbols.Dead : Symbols.Player;
        private List<IEntityAction> _actions;
        private List<IMappable> _memory = new List<IMappable>();

        public Player(string name, int level, char gender, int[] abilityScoreValues, 
                Race race, Caste caste, int team = 0, MapPoint location = null, List<Item> items = null) : 
        base(name, level, gender, race, caste, team, abilityScoreValues, location, items)
        {
            Team = team;
            _actions = new List<IEntityAction>
            {
                new TargetedAction("attack", $"[target name] - Attack a target creature within {_attackRangeFeet} feet.", "major", Attack),
                new NonTargetedAction("search", "- Search surroundings for items and creatures.", "minor", Search),
                new NonTargetedAction("hide", "- Attempt to hide from enemies", "major", Hide),
                new TargetedAction("take", "[item name] - Pick up an item within 5 feet.", "minor", TakeItem),
                new TargetedAction("drop", "[item name] - Drop an item in inventory.", "minor", DropItem),
                new TargetedAction("use", "[item name] - Use an item in inventory.", "major", UseItem),
                new TargetedAction("move", "[direction] [distance] - Move to an unoccupied space. Enter abbreviated direction and distance in feet. (e.g., NW 20)", "move", Move),
                Caste.Action,
                Race.Action
            };
            AddRangeToMemory(Items.Cast<IMappable>().ToList());
        }
        
        private void printActions(List<string> actionsRemaining)
        {
            foreach (var action in _actions) 
                {
                    if ((action == Caste.Action && CasteActionUses <= 0) || (action == Race.Action && RaceActionUses <= 0)) 
                    {
                        continue;
                    }
                    else if (actionsRemaining.Contains(action.Type))
                    {
                        int usesLeft = action == Caste.Action ? CasteActionUses : (action == Race.Action ? RaceActionUses : -1);
                        string usesLeftText = usesLeft > -1 ? $", {usesLeft} uses left" : "";
                        Console.WriteLine($"- {action.Command} {action.Description} ({action.Type}{usesLeftText})");
                    }
                }
        }  
        private void listObjectsDirectionAndDistance(List<IMappable> objects)
        {
            foreach (var obj in objects)
            {
                var nObj = (INamed)obj;
                Console.WriteLine($"- {nObj.Name} located {Math.Round(Location.DistanceTo(obj.Location)*5)} feet {Location.DirectionTo(obj.Location)}");
            }
        }
        private void describe(string targetName)
        {
            IMappable target = _memory.FirstOrDefault(t => 
                t is IDescribable && (t as INamed).Name.ToLower() == targetName.ToLower());
            if (target != null)
            {
                Console.WriteLine((target as IDescribable).Description);
            }
            else
            {
                Console.WriteLine($"{Name} does not know of {targetName.IndefiniteArticle().ToLower()} {targetName}");
            }
        }
        private void showLegend()
        {
            Console.WriteLine("Map Legend:");
            Console.WriteLine($"{Symbols.Player}  - player character");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write(Symbols.Player);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"  - player character who is taking their turn");
            Console.Write(Symbols.Dead);
            Console.WriteLine("  - dead body");
            Console.WriteLine($"{Symbols.Item}  - item that may be picked up");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(Symbols.Barrier);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("  - wall or other barrier");
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.Write(" ");
            Console.BackgroundColor = default;
            Console.WriteLine("  - blood splatter");
            Console.ResetColor();
            
        }
        public void AddToMemory(IMappable obj)
        {
            if (!_memory.Contains(obj)) _memory.Add(obj);
        }
        public void AddRangeToMemory(List<IMappable> objects)
        {
            foreach (var obj in objects) AddToMemory(obj);
        }
        private void recallMemory()
        {
            foreach (var item in Items) AddToMemory(item);
            var objectsOnMap = _memory.Where(o => Location.Map.Objects.Contains(o)).ToList();
            listObjectsDirectionAndDistance(objectsOnMap);
            foreach (var obj in _memory.Where(o => !objectsOnMap.Contains(o)))
            {
                var nObj = (INamed)obj;
                Console.WriteLine($"- {nObj.Name}");
            }
        }
        public override void TakeTurn(Combat combat)
        {
            TakingTurn = true;
            MaintainStatusEffects();
            var actionsRemaining = new List<string>() {"major", "minor", "minor", "move"};
            _movementRemaining = MovementSpeedFeet;
            bool turnOver = false;
            while (!turnOver)
            {
                if (_movementRemaining <= 0) actionsRemaining.Remove("move");
                // Check if turn is over:
                if (IsDead)
                {
                    Console.WriteLine($"{Name}'s turn is over because {Pronouns[0].ToLower()} has died!");
                    PressAnyKeyToContinue();
                    Console.Clear();
                    turnOver = true;
                    break;
                }
                else if (actionsRemaining.Count == 0)
                {
                    Console.WriteLine($"{Name}'s turn is over because {Pronouns[0].ToLower()} has no actions left.");
                    PressAnyKeyToContinue();
                    Console.Clear();
                    turnOver = true;
                    break;
                }

                Location.Map.PrintMap();

                // Prompt player:
                string formattedMovement = _movementRemaining != 0 ? $"({_movementRemaining} ft) " : "";
                Console.WriteLine($"\n{Name}'s turn: Enter an action or type 'pass' to pass your turn. {Name} has a {actionsRemaining.FormatToString("and")} action {formattedMovement}remaining.\n");
                // List IEntityActions:
                printActions(actionsRemaining);
                Console.WriteLine($"\nTry 'help' for a list of other commands.\n");

                string input = Console.ReadLine().ToLower();
                // If IEntityAction command, parse input:
                
                if (_actions.Any(a => a.Command == input.Split(' ')[0]))
                {
                    string inputCommand = input.Split(' ')[0];
                    IEntityAction inputAction = _actions.FirstOrDefault(a => a.Command == inputCommand);
                    if ((inputAction == Caste.Action && CasteActionUses > 0) || 
                        (inputAction == Race.Action && RaceActionUses > 0) ||
                        (inputAction != Caste.Action && inputAction != Race.Action))
                    {
                        if (actionsRemaining.Contains(inputAction.Type))
                        {
                            bool actionExecuted = false;
                            if (inputAction is TargetedAction)
                            {
                                var inputTargetedAction = (TargetedAction)inputAction;
                                string inputTarget = string.Join(' ', input.Split(' ').Skip(1));
                                actionExecuted = inputTargetedAction.Execute(inputTarget);
                            }
                            else if (inputAction is NonTargetedAction)
                            {
                                var inputNonTargetedAction = (NonTargetedAction)inputAction;
                                actionExecuted = inputNonTargetedAction.Execute();
                            }
                            if (actionExecuted) 
                            {
                                actionsRemaining.Remove(inputAction.Type);
                                if (inputAction == Caste.Action) CasteActionUses -= 1;
                                else if (inputAction == Race.Action) RaceActionUses -= 1;
                            }
                        }
                        else
                        {
                            Console.WriteLine($"{Name} does not have a {inputAction.Type} action remaining.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"{Name} has already expended {Pronouns[2]} uses of {inputAction.Command}.");
                    }
                }
                // Check for other commands:
                else if (input == "stats")
                {
                    Console.WriteLine(GetAllStats());
                }
                else if (input == "recall")
                {
                    recallMemory();
                }
                else if (input.Split(' ')[0] == "describe")
                {
                    describe(string.Join(' ', input.Split(' ').Skip(1)));
                }
                else if (input == "order")
                {
                    Console.WriteLine(combat.GetInitiativeOrder());
                }
                else if (input == "legend")
                {
                    showLegend();
                }
                else if (input == "help")
                {
                    Console.WriteLine($"- stats - List {Name}'s stats.");
                    Console.WriteLine($"- recall - List all items and creature in {Name}'s memory.");
                    Console.WriteLine($"- describe [target name] - Describe an item or creature in {Name}'s memory.");
                    Console.WriteLine("- order - Show current initiative order.");
                    Console.WriteLine("- legend - Show map legend.");
                }
                else if (input == "pass")
                {
                    Console.WriteLine($"{Name} is passing {Pronouns[2].ToLower()} turn.");
                    turnOver = true;
                }
                else 
                {
                    Console.WriteLine($"'{input}' could not be recognized. Please enter a valid command.");
                }
                PressAnyKeyToContinue();
                Console.Clear();
            }
            Console.Clear();
            TakingTurn = false;
        }
        
        // =======================================================================================
        // ACTIONS:
        // (each action returns true if it was successful)
        // =======================================================================================
        
        // Major actions:
        public override bool Hide()
        {
            if (HiddenDc <= 0)
            {
                return base.Hide();
            }
            else
            {
                Console.WriteLine($"{Name} is already hiding.");
                return false;
            }
        }
        // Minor actions:
        public override bool Search()
        {
            var foundObjects = BaseSearch();

            if (foundObjects.Count == 0)
            {
                Console.WriteLine($"{Name} searched but couldn't find anything!");
            }
            else
            {
                Console.WriteLine($"{Name} searched and found:");
                listObjectsDirectionAndDistance(foundObjects);
                AddRangeToMemory(foundObjects);
            }
            return true;
        }

        // Move actions:
    }
}