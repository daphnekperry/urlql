using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace urlql.mergepatch.Internal
{
    public static class StringExtensions
    {
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
