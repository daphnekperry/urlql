using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace urlql.Expressions
{
    /// <summary>
    /// The type of a Logical operation.
    /// </summary>
    public enum LogicalOperationType
    {
        /// <summary>
        /// Undefined
        /// </summary>
        [Description("Undefined")]
        Undefined = 0,

        /// <summary>
        /// And
        /// </summary>
        [Description("And")]
        And,

        /// <summary>
        /// or
        /// </summary>
        [Description("Or")]
        Or
    }
}
