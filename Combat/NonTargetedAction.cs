using System;

namespace DungeonCrawler
{
    public class NonTargetedAction : IEntityAction
    {
        public string Command {get; private set;}
        public string Description {get; private set;}
        // Returns true if successful:
        public Func<bool> Execute {get; private set;}
        public string Type {get; private set;}
        public bool Expendable {get; private set;}

        public NonTargetedAction(string command, string description, string type, Func<bool> execute, bool expendable = false)
        {
            Command = command;
            Description = description;
            Execute = execute;
            Type = type;
            Expendable = expendable;
        }
    }
}