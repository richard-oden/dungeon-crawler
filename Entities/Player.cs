using System;
using System.Collections.Generic;

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
            var actions = new List<string>() {"major", "minor", "move"};
            bool turnOver = false;
            while (!turnOver)
            {
                if (actions.Count <= 0) turnOver = true;
                string actionsString = "";
                for (int i = 0; i < actions.Count; i++) 
                {
                    actionsString += actions[i];
                    if (i != actions.Count-1) actionsString += ", ";
                    if (i == actions.Count-2) actionsString += "or ";
                }
                Console.WriteLine("Select a type of action or type 'skip' to skip your turn: " + actionsString);
                string actionInput = Console.ReadLine();
                switch (actionInput.ToLower())
                {
                    case "major":
                        
                        break;
                    case "minor":
                        
                        break;
                    case "move":
                        int movementRemaining = MovementSpeedFeet;
                        while (movementRemaining > 0)
                        {
                            Console.WriteLine("Enter the direction and distance in feet (e.g., 'N 20', 'SE 5', etc), or enter 'Q' to stop moving.");
                            Console.WriteLine($"{Name} has {movementRemaining} feet of movement left.");
                            Location.Map.PrintMap();
                            string[] moveInput = Console.ReadLine().ToLower().Split(' ');
                            if (moveInput[0] == "q") break;
                            int distance = int.Parse(moveInput[1]);
                            if (movementRemaining - distance >= 0)
                            {
                                if (Move(moveInput[0], distance))
                                {
                                    Console.Clear();
                                    movementRemaining -= distance;
                                }
                            }
                            else
                            {
                                Console.WriteLine($"{Name} cannot move {distance} feet, because {Pronouns[0].ToLower()} only has {movementRemaining} feet of movement left.");
                            }
                        }
                        break;
                    case "skip":
                        turnOver = true;
                        break;
                    default:
                        Console.WriteLine($"'{actionInput}' is not a valid action type. Please select a valid type.");
                        break;
                }
                if (actions.Contains(actionInput)) actions.Remove(actionInput);
            }
        }
    }
}