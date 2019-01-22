using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace urlql.Expressions
{
    /// <summary>
    /// Alias Operation for renaming or naming a returned value within an ISelectionExpressionStatement
    /// </summary>
    public class AliasOperation : IOperation
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
        /// As (Aliased)
        /// </summary>
        public static readonly AliasOperation Alias = new AliasOperation("as", "{0} as {1}", "As");

        /// <summary>
        /// Defined operation keywords
        /// </summary>
        public static IEnumerable<string> Keywords => new List<string>() { AliasOperation.Alias.Keyword };

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="operationName"></param>
        protected AliasOperation(string keyword, string expression, string name)
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
