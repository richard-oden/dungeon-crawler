using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonCrawler
{
    public class Map
    {
        public int Width {get; private set;}
        public int Height {get; private set;}
        public List<IMappable> Objects {get; set;}
        // {
        //     get
        //     {
        //         return Objects;
        //     }
        //     set
        //     {
        //         if (value.Any(o => !OnMap(o.Location)))
        //         {
        //             throw new OutOfMapBoundsException(this + " is outside the boundaries of the map.");
        //         }
        //         else
        //         {
        //             Objects = value;
        //         }
        //     }
        // }
        
        public Map(int width, int height, List<IMappable> objects)
        {
            Width = width;
            Height = height;
            Objects = objects;
        }
        
        public bool OnMap(Point point)
        {
            return point.X >= 0 && point.X < Width && 
                   point.Y >= 0 && point.Y < Height;
        }

        public void PrintMap()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var thisObject = Objects.SingleOrDefault(o => o.Location.X == x && o.Location.Y == y);
                    if (thisObject != null)
                    {
                        Console.Write(thisObject.Symbol + " ");
                    }
                    else
                    {
                        Console.Write("  ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}