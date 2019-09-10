using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace urlql
{
    /// <summary>
    /// Options used by the Query Resolver for yielding a query result
    /// </summary>
    public class QueryOptions
    {
        /// <summary>
        /// Absolute Maximum Page Size
        /// </summary>
        public static readonly int AbsoluteMaxPageSize = UInt16.MaxValue;
        // TODO: throw ArgumentOutOfRangeException when page sizes are set to values larger than this.

        private static int _maxPageSize = Int16.MaxValue;

        private static int _pageSize = 100;

        private static bool _requirePaging = false;

        private static CultureInfo _CultureInfo = CultureInfo.InvariantCulture;

        private static NumberStyles _NumberStyles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign;

        private static DateTimeStyles _DateTimeStyles = DateTimeStyles.AssumeLocal; // Style of the Instantiated Type(s).

        private static DateTimeKind _DateTimeKind = DateTimeKind.Utc; // Kind for the backing store/database.

        private static string[] _DateTimeFormats = {
            "yyyy-MM-ddTHH:mm:ss.FFFFFFF",  // effectively "s" but handles arbitrary length fractions of second
            "yyyy-MM-ddTHH:mm:ss.FFFFFFFK", // effectively "o" but handles arbitrary length fractions of second
            "yyyy-MM-dd",
            "HH:mm:ss.FFFFFFF",
            "HH:mm:ss.FFFFFFFK"
        };

        /// <summary>
        /// Global Default Maximum Page Size
        /// </summary>
        public static int DefaultMaximumPageSize { get; set; } = _maxPageSize;

        /// <summary>
        /// Maximum Page Size
        /// </summary>
        public int MaximumPageSize { get; set; }


        /// <summary>
        /// Global Default Page Size
        /// </summary>
        public static int DefaultPageSize { get; set; } = _pageSize;

        /// <summary>
        /// Page Size
        /// </summary>
        public int PageSize { get; set; }


        /// <summary>
        /// Global Default Require Paging
        /// </summary>
        public static bool DefaultRequirePaging { get; set; } = _requirePaging;

        /// <summary>
        /// Require Paging
        /// </summary>
        public bool RequirePaging { get; set; }


        /// <summary>
        /// Global Default Culture Info
        /// </summary>
        public static CultureInfo DefaultCultureInfo { get; set; } = _CultureInfo;

        /// <summary>
        /// Culture Info
        /// </summary>
        public CultureInfo CultureInfo { get; set; }


        /// <summary>
        /// Global Default Number Styles
        /// </summary>
        public static NumberStyles DefaultNumberStyles { get; set; } = _NumberStyles;

        /// <summary>
        /// Number Styles
        /// </summary>
        public NumberStyles NumberStyles { get; set; }


        /// <summary>
        /// Global Default Date Time Formats
        /// </summary>
        public static string[] DefaultDateTimeFormats { get; set; } = _DateTimeFormats;

        /// <summary>
        /// Date Time Formats
        /// </summary>
        public string[] DateTimeFormats { get; set; }


        /// <summary>
        /// Global Default Date Time Styles
        /// </summary>
        public static DateTimeStyles DefaultDateTimeStyles { get; set; } = _DateTimeStyles;

        /// <summary>
        /// Date Time Styles
        /// </summary>
        public DateTimeStyles DateTimeStyles { get; set; }


        /// <summary>
        /// Global Default Date Time Kind
        /// </summary>
        public static DateTimeKind DefaultDateTimeKind { get; set; } = _DateTimeKind;

        /// <summary>
        /// Date Time Kind
        /// </summary>
        public DateTimeKind DateTimeKind { get; set; }


        /// <summary>
        /// Constructor
        /// </summary>
        public QueryOptions()
        {
            MaximumPageSize = DefaultMaximumPageSize;
            PageSize = DefaultPageSize;
            RequirePaging = DefaultRequirePaging;
            CultureInfo = DefaultCultureInfo;
            NumberStyles = DefaultNumberStyles;
            DateTimeFormats = DefaultDateTimeFormats;
            DateTimeStyles = DefaultDateTimeStyles;
            DateTimeKind = DefaultDateTimeKind;
        }
    }
}
