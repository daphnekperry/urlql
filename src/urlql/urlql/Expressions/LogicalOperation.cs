using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using urlql.Internal;

namespace urlql.Expressions
{
    /// <summary>
    /// Conditional Operation
    /// </summary>
    public class LogicalOperation : IOperation
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
        /// Enumerable value for the operation.
        /// </summary>
        public LogicalOperationType OperationType { get; protected set; }

        /// <summary>
        /// and (&amp;&amp;)
        /// </summary>
        public static readonly LogicalOperation and = new LogicalOperation("and", LogicalOperationType.And, "&&");
        /// <summary>
        /// or (||)
        /// </summary>
        public static readonly LogicalOperation or = new LogicalOperation("or", LogicalOperationType.Or, "||");

        /// <summary>
        /// Operation definitions for Expression/Statement parsing
        /// </summary>
        protected static readonly ReadOnlyDictionary<string, LogicalOperation> definitions = new ReadOnlyDictionary<string, LogicalOperation>(new Dictionary<string, LogicalOperation>()
        {
            {"and", and },
            {"or", or }
        });

        /// <summary>
        /// Defined operation keywords
        /// </summary>
        public static IEnumerable<string> Keywords => definitions.Select(d => d.Key);

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="comparison"></param>
        protected LogicalOperation(string keyword, LogicalOperationType type, string expression)
        {
            Keyword = keyword;
            OperationType = type;
            Expression = expression;
            Name = Enumerations.GetEnumDescription(type);
        }

        /// <summary>
        /// Get a <see cref="LogicalOperation"/> by it's <see cref="Name"/>. Used for expression/statement parsing.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static LogicalOperation GetLogicalByName(string name)
        {
            var operation = definitions.Where(d => d.Key == name.ToLowerInvariant().Trim()).SingleOrDefault();
            return operation.Value;
        }

        /// <summary>
        /// Get a <see cref="LogicalOperation"/> by it's <see cref="OperationType"/> property. Used for statement/expression parsing.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static LogicalOperation GetLogicalByType(LogicalOperationType type)
        {
            var operation = definitions.Where(d => d.Value.OperationType == type).SingleOrDefault();
            return operation.Value;
        }

        /// <summary>
        /// Operation Dynamic Linq statement.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Expression;
        }
    }
}

