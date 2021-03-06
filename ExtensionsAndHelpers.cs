using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DungeonCrawler
{
    static class ExtensionsAndHelpers
    {
        public static T RandomElement<T>(this IEnumerable<T> enumerable)
        {
            var rand = new Random();
            int index = rand.Next(0, enumerable.Count());
            return enumerable.ElementAt(index);
        }
        
        public static void MoveElement<T>(this List<T> list, T TElement, int distance)
        {
            if (list.Contains(TElement))
            {
                int oldIndex = list.IndexOf(TElement);
                int newIndex = oldIndex + distance;
                if (newIndex < 0) 
                {
                    newIndex = 0;
                }
                else if (newIndex > list.Count - 1) 
                {
                    newIndex = list.Count -1;
                }
                list.RemoveAt(oldIndex);
                list.Insert(newIndex, TElement);
            }
            else
            {
                throw new Exception("List does not contain element.");
            }
        }
        public static string FormatToString(this IEnumerable<string> source, string conjunction)
        {
            if (source == null || !source.Any()) return null;
            var sourceArr = source.ToArray();
            string output = "";
            for (int i = 0; i < sourceArr.Length; i++) 
            {
                output += sourceArr[i];
                if (i != sourceArr.Length-1) output += (sourceArr.Length == 2 ? " " : ", ");
                if (i == sourceArr.Length-2) output += $"{conjunction} ";
            }
            return output;
        }

        public static string FromTitleOrCamelCase(this string source)
        {
            string output = Regex.Replace(source, @"([A-Z])", " " + "$1").ToLower();
            output = Regex.Replace(output, @"_", "");
            return output;
        }

        public static string IndefiniteArticle(this string noun)
        {
            return "AEIOUaeiou".IndexOf(noun[0])
            >= 0 ? "An" : "A";
        }
    
        public static void PressAnyKeyToContinue()
        {
            Console.Write("Press any key to continue... ");
            Console.ReadKey();
            Console.WriteLine();
        }
        
        public static string RandomAdjacentDirection(this string direction)
        {
            if (direction.Length == 2)
            {
                return direction.RandomElement().ToString();
            }
            else if (direction.Length == 1)
            {
                int coinFlip = Dice.Coin.Roll();
                if ("NnSs".IndexOf(direction) >= 0)
                {
                    direction.Insert(1, coinFlip == 1 ? "e" : "w");
                    return direction;
                }
                else if ("WwEe".IndexOf(direction) >= 0)
                {
                    direction.Insert(0, coinFlip == 1 ? "n" : "s");
                    return direction;
                }
                else
                {
                    throw new InvalidDirectionException($"{direction} is not a valid direction. Must contain only n, s, e, or w.");
                }
            }
            else
            {
                throw new InvalidDirectionException($"{direction} is not a valid direction. Must be 1-2 characters in length.");
            }
        }
        
        public static MapPoint ToMapPoint(this int[] coordinates, Map map)
        {
            if (coordinates.Length == 2 && coordinates != null)
            {
                return new MapPoint(coordinates[0], coordinates[1], map);
            }
            else
            {
                throw new Exception($"Improperly formatted coordinates. Coordinates must be an array of 2 integers.");
            }
        }
    }
}