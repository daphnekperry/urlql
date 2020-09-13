using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

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

        /// <summary>
        /// Case Insensitive Comparison with optional ClutureInfo
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        /// <remarks>Defaults to CultureInfo.InvariantCulture</remarks>
        public static bool CompareCaseInsensitive(this string str1, string str2, CultureInfo culture = null)
        {
            return (string.Compare(str1, str2, true, culture ?? CultureInfo.InvariantCulture)) == 0;
        }
    }
}
