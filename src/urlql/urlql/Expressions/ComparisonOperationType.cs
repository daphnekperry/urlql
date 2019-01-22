using System.ComponentModel;

namespace urlql.Expressions
{
    /// <summary>
    /// The type of a comparison operation.
    /// </summary>
    public enum ComparisonOperationType
    {
        /// <summary>
        /// Undefined operation
        /// </summary>
        [Description("Undefined")]
        Undefined = 0,

        /// <summary>
        /// Equal
        /// </summary>
        [Description("Equal")]
        Equal,

        /// <summary>
        /// Not equal
        /// </summary>
        [Description("Not equal")]
        NotEqual,

        /// <summary>
        /// Less than
        /// </summary>
        [Description("Less than")]
        LessThan,

        /// <summary>
        /// Greater than
        /// </summary>
        [Description("Greater than")]
        GreaterThan,

        /// <summary>
        /// Less than or equal
        /// </summary>
        [Description("Less than or equal")]
        LessThanOrEqual,

        /// <summary>
        /// Greater than or equal
        /// </summary>
        [Description("Greater than or equal")]
        GreaterThanOrEqual,

        /// <summary>
        /// Contains text
        /// </summary>
        [Description("Contains text")]
        ContainsText,

        /// <summary>
        /// Does not contain text
        /// </summary>
        [Description("Does not contain text")]
        DoesNotContainText,

        /// <summary>
        /// Starts with text
        /// </summary>
        [Description("Starts with text")]
        StartsWithText,

        /// <summary>
        /// Ends with text
        /// </summary>
        [Description("Ends with text")]
        EndsWithText,

        /// <summary>
        /// Equal (case insensitive)
        /// </summary>
        [Description("Equal (case insensitive")]
        EqualCI,

        /// <summary>
        /// Not equal (case insensitive)
        /// </summary>
        [Description("Not equal (case insensitive)")]
        NotEqualCI,

        /// <summary>
        /// Contains text (case insensitive)
        /// </summary>
        [Description("Contains text (case insensitive)")]
        ContainsTextCI,

        /// <summary>
        /// Does not contain text (case insensitive)
        /// </summary>
        [Description("Does not contain text (case insensitive)")]
        DoesNotContainTextCI,

        /// <summary>
        /// Starts with text (case-insensitive)
        /// </summary>
        [Description("Starts with text (case insensitive)")]
        StartsWithTextCI,

        /// <summary>
        /// Ends with text (case insensitive)
        /// </summary>
        [Description("Ends with text (case insensitive)")]
        EndsWithTextCI
    }
}
