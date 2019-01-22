using System;
using System.Collections.Generic;
using System.Text;

namespace urlql.Expressions
{
    /// <summary>
    /// Operation Interface
    /// </summary>
    public interface IOperation
    {
        /// <summary>
        /// Keyword in an expression
        /// </summary>
        string Keyword { get; }

        /// <summary>
        /// Descriptive Name for the operation.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The operation Dynamic Linq statement template.
        /// </summary>
        string Expression { get; }
    }
}
