using System;

namespace DungeonCrawler
{
    public class TargetedAction : IEntityAction
    {
        public string Command {get; private set;}
        public string Description {get; private set;}
        // Returns true if successful:
        public Func<string, bool> Execute {get; private set;}
        public string Type {get; private set;}

        public TargetedAction(string command, string description, string type, Func<string, bool> execute)
        {
            Command = command;
            Description = description;
            Execute = execute;
            Type = type;
        }
    }
}