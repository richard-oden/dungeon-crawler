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
        private List<int[]> _bloodSplatterCoordinates = new List<int[]>();
        
        public Map()
        {
            Objects = new List<IMappable>();
        }

        public static Map CsvToMap(string csvFileName, List<Item> itemList, List<Entity> entityList)
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
                    else if (locationType.Split(' ')[0] == "Spawn")
                    {
                        string objectToSpawn = locationType.Split(' ')[1]; 
                        if (objectToSpawn == "Item" && itemList.Count > 0)
                        {
                            Item randomItem = itemList.RandomElement();
                            itemList.Remove(randomItem);
                            randomItem.SetLocation(new MapPoint(x, y, newMap));
                            newMap.Objects.Add(randomItem);
                        }
                        else if (objectToSpawn == "Player" && entityList.Count > 0)
                        {
                            var playerList = (from e in entityList where e is Player select (Player)e).ToList();
                            Player randomPlayer = playerList.RandomElement();
                            entityList.Remove(randomPlayer);
                            randomPlayer.SetLocation(new MapPoint(x, y, newMap));
                            newMap.Objects.Add(randomPlayer);
                        }
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
            Objects.Add(obj);
            validateObjects(Objects);
        }

        public void AddObjects(List<IMappable> objects)
        {
            Objects.AddRange(objects);
            validateObjects(Objects);
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
        public int[][] GetCoordinatesWithin(int xStart, int xEnd, int yStart, int yEnd)
        {
            var coords = new List<int[]>();
            for (int x = xStart; x <= xEnd; x++)
            {
                for (int y = yStart; y <= yEnd; y++)
                {
                    coords.Add(new[] {x, y});
                }
            }
            return coords.ToArray();
        }

        public void CreateBloodSplatter(MapPoint location)
        {
            int numberOfSplats = Dice.D4.Roll();
            var splats = new List<int[]>();
            for (int i = 0; i < numberOfSplats; i++)
            {
                int[] splat = GetCoordinatesWithin(location.X-1, location.X+1, location.Y-1, location.Y+1).RandomElement();
                if (!splats.Contains(splat)) splats.Add(splat);
            }
            foreach (var splat in splats) if (!_bloodSplatterCoordinates.Contains(splat)) _bloodSplatterCoordinates.Add(splat);
        }
        public void PrintMap()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    ConsoleColor fgColor = DarkGray;
                    if (_bloodSplatterCoordinates.Any(c => c[0] == x && c[1] == y))
                    {
                        Console.BackgroundColor = DarkRed;
                    }
                    var thisObject = Objects.FirstOrDefault(o => o.Location.X == x && o.Location.Y == y);
                    if (thisObject != null)
                    {
                        if (thisObject is Entity || thisObject is Item)
                        {
                            fgColor = White;
                            if ((thisObject is Entity) && (thisObject as Entity).TakingTurn)
                            {
                                fgColor = DarkBlue;
                            }
                        }
                        Console.ForegroundColor = fgColor;
                        Console.Write(thisObject.Symbol + " ");
                    }
                    else
                    {
                        Console.Write("  ");
                    }
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }
    }
}