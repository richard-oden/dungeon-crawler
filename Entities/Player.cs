using System;
using System.Collections.Generic;
using System.Linq;

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
                // Check if turn is over:
                if (_movementRemaining <= 0) actionsRemaining.Remove("move");
                if (actionsRemaining.Count == 0)
                {
                    Console.WriteLine($"{Name}'s turn is over because {Pronouns[0]} has no actions left.");
                    turnOver = true;
                }

                Location.Map.PrintMap();

                // Prompt player:
                Console.WriteLine($"\nEnter an action or type 'skip' to skip your turn. {Name} has a {actionsRemaining.FormatToString("and")} action remaining.\n");
                foreach (var action in Actions) 
                {
                    if (actionsRemaining.Contains(action.Type)) Console.WriteLine($"- {action.Command} {action.Description} ({action.Type})");
                }
                Console.WriteLine("- move [direction] [distance] - Move to an unoccupied space. Enter abbreviated direction and distance in feet. (e.g., NW 20)");

                string input = Console.ReadLine().ToLower();
                if (Actions.Any(a => a.Command == input.Split(' ')[0]))
                {
                    string inputCommand = input.Split(' ')[0];
                    IEntityAction inputAction = Actions.FirstOrDefault(a => a.Command == inputCommand);
                    if (actionsRemaining.Contains(inputAction.Type))
                    {
                        if (inputAction is TargetedAction)
                        {
                            var inputTargetedAction = (TargetedAction)inputAction;
                            string inputTarget = string.Join(' ', input.Split(' ').Skip(1));
                            if (inputTargetedAction.Execute(inputTarget)) actionsRemaining.Remove(inputAction.Type);
                        }
                        else if (inputAction is NonTargetedAction)
                        {
                            var inputNonTargetedAction = (NonTargetedAction)inputAction;
                            if (inputNonTargetedAction.Execute()) actionsRemaining.Remove(inputAction.Type);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"{Name} does not have a {inputAction.Type} action remaining.");
                    }
                }
                else if (input == "skip")
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
        }

        public override void DoMove(string input)
        {
            var inputArr = input.ToLower().Split(' ');
            if (inputArr.Length == 2)
            {
                string direction = inputArr[0];
                int distance = int.Parse(inputArr[1]);
                if (_movementRemaining - distance >= 0)
                {
                    if (Move(direction, distance)) _movementRemaining -= distance;
                }
                else
                {
                    Console.WriteLine($"{Name} cannot move {distance} feet, because {Pronouns[0].ToLower()} only has {_movementRemaining} feet of movement left.");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine($"Input {input} is not valid.");
                Console.ReadKey();
            }
            Console.Clear();
        }
    }
}