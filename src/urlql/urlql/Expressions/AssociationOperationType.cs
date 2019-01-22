using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace urlql.Expressions
{
    /// <summary>
    /// The type of an Association operation.
    /// </summary>
    public enum AssociationOperationType
    {
        /// <summary>
        /// Undefined
        /// </summary>
        [Description("Undefined")]
        Undefined = 0,

        /// <summary>
        /// And
        /// </summary>
        [Description("(")]
        OpenParenthesis,

        /// <summary>
        /// or
        /// </summary>
        [Description(")")]
        CloseParenthesis,
    }
}