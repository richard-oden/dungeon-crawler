using System;
using System.Linq;
using System.Collections.Generic;

namespace DungeonCrawler
{
    public class MapPoint
    {
        public int X {get; private set;}
        public int Y {get; private set;}
        public Map Map {get; private set;}
        public MapPoint ShallowCopy => (MapPoint) this.MemberwiseClone();
        
        public MapPoint(int x, int y, Map map)
        {
            X = x;
            Y = y;
            Map = map;
        }
      
        public override string ToString()
        {
            return X + "," + Y;
        }
      
        public override bool Equals(object obj)
        {
            if (!(obj is MapPoint))
            {
                return false;
            }
            MapPoint that = obj as MapPoint;
            return (this.X == that.X) && (this.Y == that.Y);
        }
      
        public override int GetHashCode()
        {
            return X.GetHashCode() * 31 + Y.GetHashCode();
        }
        
        public int DistanceTo(int x, int y)
        {
            return (int)Math.Sqrt(Math.Pow(X-x, 2) + Math.Pow(Y-y, 2));
        }
        
        public int DistanceTo(MapPoint point)
        {
            return DistanceTo(point.X, point.Y);
        }

        public bool InRangeOf(MapPoint point, int range)
        {
            return DistanceTo(point) <= range;
        }

        public List<IMappable> GetObjectsWithinRange(int range)
        {
            return Map.Objects.Where(o => InRangeOf(o.Location, range)).ToList();
        }

        public void Translate(string direction, int distance)
        {
            switch (direction.ToLower())
            {
                case "n":
                    Y -= distance;
                    break;
                case "s":
                    Y += distance;
                    break;
                case "e":
                    X -= distance;
                    break;
                case "w":
                    X += distance;
                    break;
                case "ne":
                    Y -= distance;
                    X -= distance;
                    break;
                case "nw":
                    Y -= distance;
                    X += distance;
                    break;
                case "se":
                    Y += distance;
                    X -= distance;
                    break;
                case "sw":
                    Y += distance;
                    X += distance;
                    break;
                default:
                    Console.WriteLine($"'{direction}' is not a valid direction. Should be abbreviated as follows: 'north' = 'n', 'southeast' = 'se', etc.");
                    break;
            }
        }
    }
}