using System;
using System.Collections.Generic;
using System.Linq;
using static DungeonCrawler.ExtensionsAndHelpers;
using static DungeonCrawler.Dice;

namespace DungeonCrawler
{
    public class Player : SentientCreature
    {
        public override char Symbol => IsDead ? Symbols.Dead : Symbols.Player;
        public Player(string name, int level, char gender, int[] abilityScoreValues, Race race, Caste caste, int team = 0, MapPoint location = null) : 
        base(name, level, gender, race, caste, team, abilityScoreValues, location)
        {
            Team = team;
        }

        public override string GetDescription()
        {
            return $"{Name} is a level {Level.Value} {Race.Name} {Caste.Name}. {Pronouns[2]} hit die is a d{_hitDie.NumSides.Value}, and {Pronouns[2].ToLower()} total HP is {Hp}.";
        }

        public override void TakeTurn(Combat combat)
        {
            var actionsRemaining = new List<string>() {"major", "minor", "move"};
            _movementRemaining = MovementSpeedFeet;
            bool turnOver = false;
            while (!turnOver)
            {
                MaintainStatusEffects();
                TakingTurn = true;
                // Check if turn is over:
                if (_movementRemaining <= 0) actionsRemaining.Remove("move");
                if (actionsRemaining.Count == 0)
                {
                    Console.WriteLine($"{Name}'s turn is over because {Pronouns[0].ToLower()} has no actions left.");
                    PressAnyKeyToContinue();
                    turnOver = true;
                }

                Location.Map.PrintMap();

                // Prompt player:
                string formattedMovement = _movementRemaining != 0 ? $"({_movementRemaining} ft) " : "";
                Console.WriteLine($"\nEnter an action or type 'pass' to pass your turn. {Name} has a {actionsRemaining.FormatToString("and")} action {formattedMovement}remaining.\n");
                // List IEntityActions:
                foreach (var action in Actions) 
                {
                    if (actionsRemaining.Contains(action.Type)) Console.WriteLine($"- {action.Command} {action.Description} ({action.Type})");
                }
                Console.WriteLine($"\nTry 'help' for a list of other commands.\n");

                string input = Console.ReadLine().ToLower();
                // If IEntityAction command, parse input:
                if (Actions.Any(a => a.Command == input.Split(' ')[0]))
                {
                    string inputCommand = input.Split(' ')[0];
                    IEntityAction inputAction = Actions.FirstOrDefault(a => a.Command == inputCommand);
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
                        PressAnyKeyToContinue();
                        if (actionExecuted) actionsRemaining.Remove(inputAction.Type);
                    }
                    else
                    {
                        Console.WriteLine($"{Name} does not have a {inputAction.Type} action remaining.");
                        PressAnyKeyToContinue();
                    }
                }
                // Check for other commands:
                else if (input == "stats")
                {
                    Console.WriteLine(GetAllStats());
                    PressAnyKeyToContinue();
                }
                else if (input == "order")
                {
                    Console.WriteLine(combat.GetInitiativeOrder());
                    PressAnyKeyToContinue();
                }
                else if (input == "help")
                {
                    Console.WriteLine($"- stats - List {Name}'s stats.");
                    Console.WriteLine($"- order - Show current initiative order.");
                    PressAnyKeyToContinue();
                }
                else if (input == "pass")
                {
                    Console.WriteLine($"{Name} is passing {Pronouns[2].ToLower()} turn.");
                    PressAnyKeyToContinue();
                    turnOver = true;
                }
                else 
                {
                    Console.WriteLine($"'{input}' could not be recognized. Please enter a valid command.");
                    PressAnyKeyToContinue();
                }
                Console.Clear();
            }
            Console.Clear();
            TakingTurn = false;
        }
        // =======================================================================================
        // ACTIONS:
        // =======================================================================================
        
        // Major actions:   
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
                foreach (var obj in foundObjects)
                {
                    var nObj = (INamed)obj;
                    Console.WriteLine($"- {nObj.Name} located {Math.Round(Location.DistanceTo(obj.Location)*5)} feet {Location.DirectionTo(obj.Location)}.");
                }
            }
            return true;
        }

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

        // Move actions:
        
        // Minor actions:
        
        // Move actions:
    }
}