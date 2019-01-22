using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace urlql.Expressions
{
    /// <summary>
    /// The type of an aggregation operation.
    /// </summary>
    public enum AggregationOperationType
    {
        /// <summary>
        /// Undefined
        /// </summary>
        [Description("Undefined")]
        Undefined = 0,

        /// <summary>
        /// Count
        /// </summary>
        [Description("Count")]
        Count,

        /// <summary>
        /// DistinctCount
        /// </summary>
        [Description("Distinct Count")]
        DistinctCount,

        /// <summary>
        /// Minimum
        /// </summary>
        [Description("Minimum")]
        Minimum,

        /// <summary>
        /// Maximum
        /// </summary>
        [Description("Maximum")]
        Maximum,

        /// <summary>
        /// Average
        /// </summary>
        [Description("Average")]
        Average,

        /// <summary>
        /// Sum
        /// </summary>
        [Description("Sum")]
        Sum
    }
}
