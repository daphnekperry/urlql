using System;
using System.Collections.Generic;
using System.Text;
using urlql.Expressions;

namespace urlql.db.Expressions
{
    public abstract class IncludeOperation : IOperation
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
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="operationName"></param>
        protected IncludeOperation(string keyword, string expression, string name)
        {
            Keyword = keyword;
            Expression = expression;
            Name = name;
        }
    }
}
