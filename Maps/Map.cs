using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using static System.ConsoleColor;

namespace DungeonCrawler
{
    public class Map
    {
        public int Width {get; private set;}
        public int Height {get; private set;}
        public List<IMappable> Objects {get; private set;}
        
        public Map()
        {
            Objects = new List<IMappable>();
        }
        
        public Map(int width, int height)
        {
            Width = width;
            Height = height;
            Objects = new List<IMappable>();
        }

        public static Map CsvToMap(string csvFileName)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            var fileName = Path.Combine(currentDirectory, csvFileName);
            var mapObjects = new List<IMappable>();
            Map newMap = new Map();
            int lineNumber = 1;
            using (var reader = new StreamReader(fileName)) // using directive closes streamreader after finished
            {
                string line = "";
                reader.ReadLine(); // consumes first line, which only contains headings
                while((line = reader.ReadLine()) != null) // while the line is not null
                {
                    string[] values = line.Split(',');
                    if (lineNumber == 1)
                    {
                        newMap.Width = int.Parse(values[3]);
                        newMap.Height = int.Parse(values[4]);
                    }
                    string locationType = values[0];
                    int x = int.Parse(values[1]);
                    int y = int.Parse(values[2]);
                    if (locationType == "Wall")
                    {
                        mapObjects.Add(new Wall(new MapPoint(x, y, newMap)));
                    }
                    lineNumber++;
                }
            }
            newMap.AddObjects(mapObjects);
            return newMap;
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

        public int[][] GetOpenSpaces(IEnumerable<int[]> coordinates)
        {
           return coordinates.Where(c => !Objects.Any(o => o.Location.X == c[0] && o.Location.Y == c[1])).ToArray();
        }

        public int[][] GetAllOpenSpaces()
        {
            var allCoordinates = new List<int[]>();
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    allCoordinates.Add(new[] {x, y});
                }
            }
            return GetOpenSpaces(allCoordinates);
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