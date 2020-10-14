using System.Collections.Generic;
using System.Linq;

namespace DungeonCrawler
{
    static class ExtensionMethods
    {
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

        public static string IndefiniteArticle(this string noun)
        {
            return "AEIOUaeiou".IndexOf(noun[0]) >= 0 ? "An" : "A";
        }
    }
}