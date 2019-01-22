using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace urlql.Expressions
{
    /// <summary>
    /// The type of an ordering operation.
    /// </summary>
    public enum OrderingOperationType
    {
        /// <summary>
        /// Undefined
        /// </summary>
        [Description("Undefined")]
        Undefined = 0,

        /// <summary>
        /// Ascending
        /// </summary>
        [Description("Ascending")]
        Ascending,

        /// <summary>
        /// Descending
        /// </summary>
        [Description("Descending")]
        Descending
    }
}
