using System;
using System.Collections.Generic;
using System.Linq;
using static DungeonCrawler.ExtensionsAndHelpers;

namespace DungeonCrawler
{
    public class Player : SentientCreature
    {
        public override char Symbol {get; protected set;} = Symbols.PlayerS;
        public Player(string name, int level, char gender, int[] abilityScoreValues, Race race, Caste caste, MapPoint location = null) : base(name, level, gender, race, caste, abilityScoreValues, location)
        {
        }

        public override string GetDescription()
        {
            return $"{Name} is a level {Level.Value} {Race.Name} {Caste.Name}. {Pronouns[2]} hit die is a d{_hitDie.NumSides.Value}, and {Pronouns[2].ToLower()} total HP is {_hp}.";
        }

        public override void TakeTurn()
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
                    Console.WriteLine($"{Name}'s turn is over because {Pronouns[0]} has no actions left.");
                    turnOver = true;
                }

                Location.Map.PrintMap();

                // Prompt player:
                string formattedMovement = _movementRemaining != 0 ? $"({_movementRemaining} ft) " : "";
                Console.WriteLine($"\nEnter an action or type 'pass' to pass your turn. {Name} has a {actionsRemaining.FormatToString("and")} action {formattedMovement}remaining.\n");
                foreach (var action in Actions) 
                {
                    if (actionsRemaining.Contains(action.Type)) Console.WriteLine($"- {action.Command} {action.Description} ({action.Type})");
                }
                Console.WriteLine($"\nEnter 'stats' at any time to view {Name}'s stats.\n");

                string input = Console.ReadLine().ToLower();
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
                    }
                }
                else if (input == "stats")
                {
                    Console.WriteLine(GetAllStats());
                    PressAnyKeyToContinue();
                }
                else if (input == "pass")
                {
                    turnOver = true;
                }
                else 
                {
                    Console.WriteLine($"'{input}' could not be recognized. Please enter a valid command.");
                }
                Console.Clear();
            }
            Console.Clear();
            TakingTurn = false;
        }
    }
}