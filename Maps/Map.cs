using System;
using System.Collections.Generic;
using System.Linq;
using static System.ConsoleColor;

namespace DungeonCrawler
{
    public class Map
    {
        public int Width {get; private set;}
        public int Height {get; private set;}
        public List<IMappable> Objects {get; private set;}
        
        public Map(int width, int height)
        {
            Width = width;
            Height = height;
            Objects = new List<IMappable>();
        }
        
        public bool OnMap(MapPoint point)
        {
            return point.X >= 0 && point.X < Width && 
                   point.Y >= 0 && point.Y < Height;
        }

        private void validateObjects(List<IMappable> value)
        {
            var objectsOutOfBounds = value.Where(o => !OnMap(o.Location)).ToArray();
            var objectDuplicates = value.GroupBy(o => o.Location)
                                        .Where(g => g.Count() > 1)
                                        .Select(g => g.Key).ToArray();
            if (objectsOutOfBounds.Length > 0)
            {
                string outOfBoundLocations = "";
                foreach (var o in objectsOutOfBounds) outOfBoundLocations += $"({o.Location.X},{o.Location.Y}) ";
                throw new OutOfMapBoundsException($"Objects at {outOfBoundLocations}are outside the boundaries of the map.");
            }
            else if (objectDuplicates.Length > 0)
            {
                string duplicateLocations = "";
                foreach (var o in objectDuplicates) duplicateLocations += $" ({o.X},{o.Y})";
                throw new DuplicateLocationException($"Duplicate locations found at{duplicateLocations}.");
            }
        }

        public void AddObject(IMappable obj)
        {
            List<IMappable> tempObjects = Objects;
            tempObjects.Add(obj);
            validateObjects(tempObjects);
            Objects.Add(obj);
        }

        public void AddObjects(List<IMappable> objects)
        {
            List<IMappable> tempObjects = Objects;
            tempObjects = tempObjects.Concat(objects).ToList();
            validateObjects(tempObjects);
            Objects = tempObjects;
        }

        public void RemoveObject(IMappable obj)
        {
            if (Objects.Contains(obj))
            {
                Objects.Remove(obj);
            }
            else
            {
                Console.WriteLine("Map does not contain object.");
            }
        }

        public void PrintMap()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    ConsoleColor consoleColor = DarkGray;
                    var thisObject = Objects.FirstOrDefault(o => o.Location.X == x && o.Location.Y == y);
                    if (thisObject != null)
                    {
                        if (thisObject is Entity)
                        {
                            var thisEntity = (Entity)thisObject;
                            if (thisEntity.TakingTurn)
                            {
                                consoleColor = DarkBlue;
                            }
                            else if (thisEntity is INpc && thisEntity.Team != 0)
                            {
                                consoleColor = (thisEntity as INpc).Aggression switch
                                {
                                    Aggression.Low => Green,
                                    Aggression.Mid => DarkYellow,
                                    Aggression.High => DarkRed,
                                    _ => throw new InvalidAggressionException($"{(thisEntity as INpc).Aggression} is an invalid aggression level.")
                                };
                            }
                        }
                        else if (thisObject is Item)
                        {
                            consoleColor = White;
                        }
                        Console.ForegroundColor = consoleColor;
                        Console.Write(thisObject.Symbol + " ");
                    }
                    else
                    {
                        Console.Write("  ");
                    }
                }
                Console.WriteLine();
                Console.ResetColor();
            }
        }
    }
}