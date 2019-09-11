using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace urlql.Internal
{
    public static class StringExtensions
    {
        public static string StringLiteralRegex => "(?<=^[^\"]*(?:\"[^\"]*\"[^\"]*)*) (?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";

        /// <summary>
        /// Tokenize the string by spaces, ignoring string expressions ("text in double quotes").
        /// </summary>
        /// <param name="str"></param>
        public static IEnumerable<string> Tokenize(this string str)
        {
            var preped = str.Replace("(", " ( ").Replace(")", " ) ").Trim();
            return Regex.Split(preped, StringLiteralRegex).Where(s => !string.IsNullOrEmpty(s.Trim())).Select(s => s.Trim());
        }
    }
}
