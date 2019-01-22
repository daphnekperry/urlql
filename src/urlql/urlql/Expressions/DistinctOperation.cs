using System;
using System.Collections.Generic;
using System.Text;

namespace urlql.Expressions
{
    /// <summary>
    /// Distinct Operation for a distinct results for a Selection Statement
    /// </summary>
    public class DistinctOperation : IOperation
    {
        /// <summary>
        /// Keyword in an expression
        /// </summary>
        public string Keyword { get; protected set; }

        /// <summary>
        /// Descriptive Name for the operation.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// The operation Dynamic Linq statement template.
        /// </summary>
        public string Expression { get; protected set; }

        /// <summary>
        /// Distinct
        /// </summary>
        public static readonly DistinctOperation Distinct = new DistinctOperation("distinct", "distinct", "Distinct");

        /// <summary>
        /// Defined operation keywords
        /// </summary>
        public static IEnumerable<string> Keywords => new List<string>() { DistinctOperation.Distinct.Keyword };

        /// <summary>
        /// Constructor
        /// </summary>
        protected DistinctOperation(string keyword, string expression, string name)
        {
            Keyword = keyword;
            Expression = expression;
            Name = name;
        }

        /// <summary>
        /// Operation as a Dynamic Linq statement
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Expression;
        }
    }
}
