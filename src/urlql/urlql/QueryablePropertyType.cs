using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace urlql
{
    /// <summary>
    /// The type of a QueryablePropertyType
    /// </summary>
    public enum QueryablePropertyType
    {
        /// <summary>
        /// Reference Type
        /// </summary>
        [Description("Reference")]
        Reference = -1,

        /// <summary>
        /// Undefined
        /// </summary>
        [Description("Undefined")]
        Undefined = 0,

        /// <summary>
        /// Operation valid for any Value type
        /// </summary>
        [Description("Any")]
        Any,

        /// <summary>
        /// Operation valid for Scalar/Quantity types
        /// </summary>
        [Description("Value")]
        Scalar,

        /// <summary>
        /// Operation valid for Numeric types
        /// </summary>
        [Description("Numeric")]
        Numeric,

        /// <summary>
        /// Operation valid for DateTime structure
        /// </summary>
        [Description("DateTime")]
        DateTime,

        /// <summary>
        /// Operation valid for Boolean types
        /// </summary>
        [Description("Boolean")]
        Boolean,

        /// <summary>
        /// Operation valid for Text types
        /// </summary>
        [Description("Text")]
        Text,

        /// <summary>
        /// Operation valid for Guid structure
        /// </summary>
        [Description("Guid")]
        Guid
    }
}
